using Godot;
using System;

public partial class HitEffect : AnimatedSprite2D
{

	public override void _Ready()
	{
		this.AnimationFinished += OnAnimationFinshed;
	}

	private void OnAnimationFinshed()
	{
		QueueFree();
	}

	public override void _Process(double delta)
	{
	}
}
