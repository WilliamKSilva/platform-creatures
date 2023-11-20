using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene LevelScene { get; set; }

	public override void _Ready()
	{
		JsonHelper.Parse("grawg");

		var signals = GetNode<Signals>("/root/Signals");
		signals.OnPlayerCollision += PlayerCollision;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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