using Godot;
using System;
using System.Linq;

public partial class Main : Node
{
	[Export]
	public PackedScene LevelScene { get; set; }

	public Node Level;

	public Dialog[] LevelDialogs = Array.Empty<Dialog>();
	public int DialogOrder = 0;

	public bool FirstDialogShowed = false;

	public string[] NPCsNames = new string[] { "Grawg" };
	public Node2D[] NPCsOnScene = Array.Empty<Node2D>();

	public override void _Ready()
	{
		// Connect signal to detect player / NPC collision
		var signals = GetNode<Signals>("/root/Signals");
		signals.OnPlayerCollision += PlayerCollision;

		// Render current level
		InstantiateLevel(LevelScene);

		// Get all NPCs on scene
		GetNPCsOnScene();

		// Set NPCs on scene position and dialogs
		SetNPCsOnScene();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Player player = GetNode<Player>("Player");
		// Start the dialog interaction from the first dialog 

		if (player.InDialog && !FirstDialogShowed)
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
		for (var i = 0; i < NPCsNames.Length; i++)
		{
			string npcName = NPCsNames[i];
			Node2D npcNode2D = Level.GetNode<Node2D>(npcName);

			if (npcNode2D != null)
			{
				NPCsOnScene = NPCsOnScene.Append(npcNode2D).ToArray();
			}
		}
	}

	public void SetNPCsOnScene()
	{
		for (var i = 0; i < NPCsOnScene.Length; i++)
		{
			Node2D npcNode2D = NPCsOnScene[i];
			SetNPCPosition(npcNode2D);
			LoadNPCDialogs(npcNode2D);
		}
	}

	public static void SetNPCPosition(Node2D npcNode2D)
	{
		Marker2D positionNode = npcNode2D.GetNode<Marker2D>("Position");
		npcNode2D.Position = new Vector2(positionNode.Position.X, positionNode.Position.Y);
	}

	// TODO: Class for this type of methods
	public void LoadNPCDialogs(Node2D npcNode2D)
	{
		Dialog[] dialogs = JsonHelper.Parse(npcNode2D.Name);

		for (int i = 0; i < dialogs.Length; i++)
		{
			Dialog dialog = dialogs[i];
			dialog.character = npcNode2D.Name;
			LevelDialogs = LevelDialogs.Append(dialog).ToArray();
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

	public void ShowDialog()
	{
		Dialog currentDialog = GetCurrentDialog();

		if (currentDialog == null)
		{
			return;
		}

		if (currentDialog.character == "Player")
		{
			return;	
		}

		Node2D npcNode2D = GetLevelNPCByName(currentDialog.character);
		Node2D npc = npcNode2D.GetNode<Node2D>("NPC");

		Control dialogBox = npc.GetNode<Control>("Dialog");
		dialogBox.Visible = true;

		Dialog dialog = LevelDialogs[DialogOrder];

		if (dialog == null)
		{
			return;
		}

		RichTextLabel dialogNode = dialogBox.GetNode<RichTextLabel>("Text");
		dialogNode.Text = dialog.text;

		FirstDialogShowed = true;
	}

	// TODO: Maybe make an class for this type of methods
	public Node2D GetLevelNPCByName(string npcName) {
		for (int i = 0; i < NPCsOnScene.Length; i++)
		{
			Node2D npcNode2D = NPCsOnScene[i];

			if (npcNode2D.Name == npcName)
			{
				return npcNode2D;
			}
		}

		return null;
	}

	public void OnPlayerDialogSkip()
	{
		DialogOrder++;
		ShowDialog();
	}

	public void PlayerCollision(Node2D npcNode2D)
	{
		Player player = GetNode<Player>("Player");
		player.InDialog = true;
	}
}