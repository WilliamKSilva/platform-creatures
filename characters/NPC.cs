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

	public void AddDialog(string text, string level) {
		Dialog dialog = new Dialog();
		dialog.text = text;
		dialog.level = level;
		Dialogs = Dialogs.Append(dialog).ToArray();
	}
}