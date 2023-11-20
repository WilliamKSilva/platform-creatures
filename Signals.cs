using Godot;
using System;

public partial class Signals : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowDialog(string dialogText, NPC npc)
	{
		RichTextLabel dialogBox = npc.GetNode<RichTextLabel>("/Dialog/Text");
		dialogBox.Visible = true;
	}

	[Signal]
	public delegate void OnPlayerCollisionEventHandler(Node2D npcNode2D);
}
