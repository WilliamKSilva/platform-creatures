using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : Node
{
	public PackedScene LevelScene { get; set; }

	public Node Level;

	public Player Player;

	public Dialog[] LevelDialogs = Array.Empty<Dialog>();
	public int DialogOrder = 0;

	public bool FirstDialogShowed = false;

	public List<string> NPCsNames = new List<string>();
	public Node2D[] CharactersOnScene = Array.Empty<Node2D>();

	public override void _Ready()
	{
		// First Scene is Introduction
		LevelScene = ResourceLoader.Load<PackedScene>("res://levels/Introduction.tscn");
		// Connect signal to detect player / NPC collision
		var signals = GetNode<Signals>("/root/Signals");
		signals.OnPlayerCollision += PlayerCollision;

		// Render current level
		InstantiateLevel(LevelScene);

		// Get all NPCs on scene
		NPCsNames.Add("Grawg");
		GetNPCsOnScene();

		SetPlayerInitialPosition();

		// Set NPCs on scene position and dialogs
		SetNPCsOnScene();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Start the dialog interaction from the first dialog 
		bool PlayerExists = Player != null;
		if (PlayerExists && Player.InDialog && !FirstDialogShowed)
		{
			ShowDialog();
		}
	}

	public void InstantiateLevel(PackedScene scene)
	{
		var sceneInstantiaded = scene.Instantiate();
		Level = sceneInstantiaded;
		AddChild(sceneInstantiaded);
	}

	public void GetNPCsOnScene()
	{
		for (var i = 0; i < NPCsNames.Count; i++)
		{
			string npcName = NPCsNames[i];
			Node2D npcNode2D = Level.GetNode<Node2D>(npcName);

			if (npcNode2D != null)
			{
				CharactersOnScene = CharactersOnScene.Append(npcNode2D).ToArray();
			}
		}

		Player player = GetNode<Player>("Player");
		Player = player;
	}

	public void SetNPCsOnScene()
	{
		for (var i = 0; i < CharactersOnScene.Length; i++)
		{
			Node character = CharactersOnScene[i];

			SetNPCPosition(character as Node2D);
			LoadNPCDialogs(character as Node2D);
		}

		LoadNPCDialogs(Player);
	}

	public static void SetNPCPosition(Node2D npcNode2D)
	{
		Marker2D positionNode = npcNode2D.GetNode<Marker2D>("Position");
		npcNode2D.Position = new Vector2(positionNode.Position.X, positionNode.Position.Y);
	}

	// TODO: Class for this type of methods
	public void LoadNPCDialogs(Node2D npcNode2D)
	{
		if (npcNode2D != null)
		{
			Dialog[] dialogs = JsonHelper.Parse(npcNode2D.Name);

			for (int i = 0; i < dialogs.Length; i++)
			{
				Dialog dialog = dialogs[i];
				dialog.character = npcNode2D.Name;
				LevelDialogs = LevelDialogs.Append(dialog).ToArray();
			}
		}
	}

	public Dialog GetCurrentDialog()
	{
		for (int i = 0; i < LevelDialogs.Length; i++)
		{
			Dialog dialog = LevelDialogs[i];

			if (dialog == null)
			{
				return null;
			}

			if (dialog.order == DialogOrder)
			{
				return dialog;
			}
		}

		return null;
	}

	public Dialog GetLastDialog()
	{
		Dialog lastDialog = LevelDialogs[0];

		for (int i = 0; i < LevelDialogs.Length; i++)
		{
			Dialog currentDialog = LevelDialogs[i];

			if (currentDialog.order > lastDialog.order)
			{
				lastDialog = currentDialog;
				continue;
			}
		}

		return lastDialog;
	}

	public void ShowDialog()
	{
		Dialog currentDialog = GetCurrentDialog();

		if (currentDialog == null)
		{
			return;
		}

		if (currentDialog.character == "Player")
		{
			CharacterBody2D player = GetNode<CharacterBody2D>("Player");
			Control playerDialogBox = player.GetNode<Control>("Dialog");
			playerDialogBox.Visible = true;
			AnimatedSprite2D animatedSprite2D = playerDialogBox.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

			switch (currentDialog.text)
			{
				case PlayerDialogTypes.QUESTION:
					animatedSprite2D.Play("question");
					break;
				case PlayerDialogTypes.LAUGH:
					animatedSprite2D.Play("laugh");
					break;
				case PlayerDialogTypes.OK:
					animatedSprite2D.Play("ok");
					break;
				default:
					break;
			}

			return;
		}

		Node2D npcNode2D = GetLevelNPCByName(currentDialog.character);
		Node2D npc = npcNode2D.GetNode<Node2D>("NPC");

		Control dialogBox = npc.GetNode<Control>("Dialog");
		dialogBox.Visible = true;

		Dialog dialog = GetCurrentDialog();

		if (dialog == null)
		{
			return;
		}

		RichTextLabel dialogNode = dialogBox.GetNode<RichTextLabel>("Text");
		dialogNode.Text = dialog.text;

		FirstDialogShowed = true;
	}

	public bool DialogsEnded()
	{
		bool lastDialogDisplayed = GetLastDialog().order == DialogOrder;

		return lastDialogDisplayed;
	}

	// TODO: Maybe make an class for this type of methods
	public Node2D GetLevelNPCByName(string npcName)
	{
		for (int i = 0; i < CharactersOnScene.Length; i++)
		{
			Node2D npcNode2D = CharactersOnScene[i];

			if (npcNode2D.Name == npcName)
			{
				return npcNode2D;
			}
		}

		return null;
	}

	public void RemoveCharactersOfScene()
	{
		for (int i = 0; i < CharactersOnScene.Length; i++)
		{
			var character = CharactersOnScene[i];
			RemoveChild(character);
		}
	}

	public void OnPlayerDialogSkip()
	{
		Player player = GetNode<Player>("Player");

		if (DialogsEnded() && player.InDialog)
		{
			Dialog lastDialogDisplayed = GetCurrentDialog();

			if (lastDialogDisplayed.character == "Player")
			{
				Control dialogBox = player.GetNode<Control>("Dialog");
				dialogBox.Visible = false;
			}
			else
			{
				Node2D npcNode2D = GetLevelNPCByName(lastDialogDisplayed.character);
				CharacterBody2D npc = npcNode2D.GetNode<CharacterBody2D>("NPC");
				Control dialogBox = npc.GetNode<Control>("Dialog");
				dialogBox.Visible = false;
			}

			player.InDialog = false;

			// After the Dialog ends turn the NPC collision of so the player can continue
			Dialog lastDialog = GetLastDialog();
			// TODO: make an function to Get the respective NPC by name
			Node2D node2D = Level.GetNode<Node2D>(lastDialog.character);
			NPC npcNode = node2D.GetNode<NPC>("NPC");

			CollisionShape2D collisionShape = npcNode.GetNode<CollisionShape2D>("CollisionShape2D");
			collisionShape.Disabled = true;

			return;
		}

		if (player.InDialog)
		{
			Dialog lastDialogDisplayed = GetCurrentDialog();
			if (lastDialogDisplayed.character == "Player")
			{
				Control dialogBox = player.GetNode<Control>("Dialog");
				dialogBox.Visible = false;
			}
			else
			{
				Node2D npcNode2D = GetLevelNPCByName(lastDialogDisplayed.character);
				CharacterBody2D npc = npcNode2D.GetNode<CharacterBody2D>("NPC");
				Control dialogBox = npc.GetNode<Control>("Dialog");
				dialogBox.Visible = false;
			}

			DialogOrder++;
			ShowDialog();
		}
	}

	public void OnPlayerOutOfScreen()
	{
		// Reset everything
		Array.Clear(LevelDialogs);
		DialogOrder = 0;
		FirstDialogShowed = false;
		CharactersOnScene = Array.Empty<Node2D>();
		NPCsNames.Clear();

		RemoveCharactersOfScene();
		RemoveChild(Level);

		LevelScene = ResourceLoader.Load<PackedScene>("res://levels/level1.tscn");

		BuildNewLevel();
	}

	public void BuildNewLevel()
	{
		// Render current level
		InstantiateLevel(LevelScene);

		// Get all NPCs on scene
		GetNPCsOnScene();

		SetPlayerInitialPosition();

		// Set NPCs on scene position and dialogs
		SetNPCsOnScene();
	}

	public void SetPlayerInitialPosition()
	{
		Marker2D playerPositionMarker = GetNode<Marker2D>("PlayerPosition"); 
		Player.Position = new Vector2(playerPositionMarker.Position.X, playerPositionMarker.Position.Y);
	}

	public void PlayerCollision(Node2D npcNode2D)
	{
		Player player = GetNode<Player>("Player");
		player.InDialog = true;
	}
}