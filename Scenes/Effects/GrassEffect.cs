using Godot;
using System;

public partial class GrassEffect : Node2D
{
	private AnimatedSprite2D animatedSprite2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	public override void _EnterTree()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		//添加信号 让动画播放结束时销毁自己
		animatedSprite2D.AnimationFinished += QueueFree;
	}

	public override void _Process(double delta)
	{
	}
}
