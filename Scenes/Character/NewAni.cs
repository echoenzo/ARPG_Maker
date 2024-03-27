using Godot;
using System;

public partial class NewAni : CharacterBody2D
{
	private float Speed = 2000;

	private Sprite2D sprite2D;
	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDirection = Vector2.Zero;
		inputDirection.X = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		inputDirection.Y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		inputDirection = inputDirection.Normalized();
		if (inputDirection != Vector2.Zero)
		{
			if (inputDirection.X > 0)
			{
				sprite2D.FlipH = true;
			}
			else
			{
				sprite2D.FlipH = false;
			}
			animationPlayer.Play("Run");
		}
		else if (inputDirection == Vector2.Zero)
		{

			animationPlayer.Play("Idle");
		}
		else
		{

		}
		if (Input.IsActionJustPressed("Space"))
		{

			animationPlayer.Play("Flating");
			Velocity = Velocity.MoveToward(5000 * inputDirection, (float)delta);

		}
		Velocity = inputDirection * Speed * (float)delta;

		MoveAndSlide();
	}
}
