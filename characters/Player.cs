using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 600;

	public int JumpSpeed = -1000;

	public int Gravity = 500;

	public bool InDialog = false;

	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("Animation").Play("player_stand");
	}

	public override void _Input(InputEvent @event)
	{
		if (InDialog)
		{
			if (@event.IsActionPressed("dialog_skip"))
			{
				EmitSignal(SignalName.OnDialogSkip);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		if (IsOutOfScreen())
		{
			EmitSignal(SignalName.OnOutOfScreen);
		}

		if (InDialog)
		{
			return;
		}

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("jump"))
		{
			velocity.Y = JumpSpeed;
		}

		if (!IsOnFloor())
		{
			velocity.Y += Gravity;
		}

		velocity.X *= Speed;
		Velocity = velocity;

		MoveAndSlide();
	}

	public bool IsOutOfScreen() 
	{
		return Position.X > 1920;
	}

	[Signal]
	public delegate void OnDialogSkipEventHandler(Node2D npcNode2D);

	[Signal]
	public delegate void OnOutOfScreenEventHandler();

}
