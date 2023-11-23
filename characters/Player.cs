using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 300;

	public int JumpSpeed = -700;
	public int Gravity = 300;

	public bool InDialog = false;

    public override void _Ready()
    {
			GetNode<AnimatedSprite2D>("Animation").Play("player_stand");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Vector2.Zero;

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
}
