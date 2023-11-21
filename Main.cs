using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene LevelScene { get; set; }

	public override void _Ready()
	{
		// JsonHelper.Parse("grawg");

		// Connect signal to detect player / NPC collision
		var signals = GetNode<Signals>("/root/Signals");
		signals.OnPlayerCollision += PlayerCollision;

		// Render current level
		InstantiateLevel(LevelScene);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void InstantiateLevel(PackedScene scene)
	{
		var sceneInstantiaded = scene.Instantiate();
		SetNPCPosition("Grawg", sceneInstantiaded);
		AddChild(sceneInstantiaded);
	}

	public void SetNPCPosition(string npcName, Node scene)
	{
		var npc = scene.GetNode<Node2D>($"{npcName}");
		Marker2D positionNode = npc.GetNode<Marker2D>("Position");
		npc.Position = new Vector2(positionNode.Position.X, positionNode.Position.Y);
	}

	public void ShowNPCDialog(string dialogText, NPC npc)
	{
		Control dialogBox = npc.GetNode<Control>("Dialog");
		dialogBox.Visible = true;

		RichTextLabel dialog = dialogBox.GetNode<RichTextLabel>("Text");
		dialog.Text = dialogText;
	}

	public void PlayerCollision(Node2D npcNode2D)
	{
		NPC npc = npcNode2D.GetNode<NPC>("NPC");
	}
}