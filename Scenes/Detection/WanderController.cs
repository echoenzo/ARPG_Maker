using Godot;
using System;

public partial class WanderController : Node2D
{
	public Vector2 StartPosition;
	public Vector2 TargetPosition;
	private Timer timer;
	[Export]
	public int randomRange;

	public override void _Ready()
	{
		StartPosition = GlobalPosition;
		TargetPosition = GlobalPosition;
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
	}
	public double GetTimeLeft()
	{
		return timer.TimeLeft;
	}
	private void OnTimerTimeout()
	{
		UpdateTargetPosition();
	}
	public void StartTimer(float duration)
	{
		timer.Start(duration);
	}
	public void UpdateTargetPosition()
	{
		Random random = new Random();
		var x = random.Next(-randomRange, randomRange);
		var y = random.Next(-randomRange, randomRange);
		var targetVector = new Vector2(x, y) + TargetPosition;
		StartPosition = (targetVector + TargetPosition) / 2;
		TargetPosition = targetVector;

	}


}
