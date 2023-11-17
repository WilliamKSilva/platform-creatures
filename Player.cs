using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 200;

	public int JumpSpeed = -400;
	public int Gravity = 200;

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

		Timer timer = GetNode<Timer>("JumpTimer");
		GD.Print(timer.TimeLeft);

		if (Input.IsActionJustPressed("jump") && timer.TimeLeft == 0)
		{
			GD.Print("Pressed");
			timer.Start();
		}

		if (Input.IsActionPressed("jump") && timer.TimeLeft > 0)
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
