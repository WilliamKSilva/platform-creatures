using Godot;
using System;
using System.Linq;

public partial class NPC : CharacterBody2D
{
	public const float Speed = 600.0f;
	public const float JumpVelocity = -400.0f;

	public Dialog[] Dialogs = Array.Empty<Dialog>();

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	// public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		AddDialog("Teste", "Introduction");
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = new Vector2(0, 0);
		// Add the gravity.
		// if (!IsOnFloor())
		// 	velocity.Y += gravity * (float)delta;
		MoveAndSlide();

		for (var index = 0; index < GetSlideCollisionCount(); index++)
		{
			KinematicCollision2D collision = GetSlideCollision(index);

			// If the collision detection is the floor, ignore
			if (collision.GetCollider() is StaticBody2D)
			{
				return;
			}

			if (collision.GetCollider() is Player)
			{
				Signals signals = GetNode<Signals>("/root/Signals");
				Node2D parentNode = GetParent<Node2D>();
				signals.EmitSignal("OnPlayerCollision", parentNode);
			}
		}
	}

	public static void SetPosition(Node2D npcNode2D)
	{
		Marker2D positionNode = npcNode2D.GetNode<Marker2D>("Position");
		npcNode2D.Position = new Vector2(positionNode.Position.X, positionNode.Position.Y);
	}

	public void AddDialog(string text, string level)
	{
		Dialog dialog = new Dialog();
		dialog.text = text;
		dialog.level = level;
		Dialogs = Dialogs.Append(dialog).ToArray();
	}

	public static Dialog[] LoadDialogs(Node2D npcNode2D, Dialog[] levelDialogs)
	{
		if (npcNode2D != null)
		{
			Dialog[] dialogs = JsonHelper.Parse(npcNode2D.Name);

			for (int i = 0; i < dialogs.Length; i++)
			{
				Dialog dialog = dialogs[i];
				dialog.character = npcNode2D.Name;
				levelDialogs = levelDialogs.Append(dialog).ToArray();
			}
		}

		return levelDialogs;
	}
}