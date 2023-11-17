using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;

	public const float Gravity = 2000.0f;

	public override void _PhysicsProcess(double delta)
	{
		if (!IsOnFloor())
		{
			MoveAndCollide(new Vector2(0, 15));	
		}
	}
}
