using Godot;
using System;
using System.Diagnostics;
using System.Reflection;

public partial class Warrior : CharacterBody2D
{
	int Max_Speed = 100;
	int acceleration = 100;//加速度
	int friction = 1000;//摩擦力
	private AnimationPlayer animationPlayer;
	private AnimatedSprite2D animatedSprite2D;
	private AnimationTree animationTree;
	private Sprite2D sprite2D;
	private Timer timer;
	private Timer timer2;
	private AnimationNodeStateMachinePlayback animationNodeState;
	private Vector2 inputDirection;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer2 = GetNode<Timer>("Timer2");
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		//animationTree = GetNode<AnimationTree>("AnimationTree");
		//animationNodeState = animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>() ;//变体的转换需要使用as和from
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		//Vector2 velocity = Vector2.Zero;
		Vector2 inputDirection = Vector2.Zero;
		inputDirection.X = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		inputDirection.Y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		//velocity = velocity.Normalized() * Max_Speed*(float)delta;
		//velocity = velocity.Normalized() * acceleration*(float)delta;
		inputDirection = inputDirection.Normalized();
		if (inputDirection != Vector2.Zero)
		{
			this.inputDirection = inputDirection;
		}
		if (inputDirection != Vector2.Zero)
		{
			if (inputDirection.X > 0)
			{
				sprite2D.FlipH = false;
			}
			else
			{
				sprite2D.FlipH = true;
			}
			animationPlayer.Play("Run");
			//如果方向值不为0时  设置animationtree的Idle的值为方向值  这样在方向值为0时 animationtree的idle值不会改变

			//animationTree.Set("parameters/Idle/blend_position", inputDirection);
			//animationTree.Set("parameters/Run/blend_position", inputDirection);
			//GD.Print(inputDirection);
			//animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>().Travel("Run");
			//animationNodeState.Travel("Run");
			Velocity = Velocity.MoveToward(inputDirection * Max_Speed, acceleration * (float)delta);
		}
		else
		{
			//animationPlayer.Play("Idle_Right");
			//animationNodeState.Travel("Idle");
			//animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>().Travel("Idle");
			Velocity = Velocity.MoveToward(Vector2.Zero, friction * (float)delta);
		}
		if (Input.IsActionJustPressed("Attack1"))
		{
			// animationPlayer.Play("Attack1");
			// if (Input.IsActionJustPressed("Attack1"))
			// {
			// 	animationPlayer.Play("Attack2");
			// }
			Attack(inputDirection);
			if (timer.IsStopped())
			{
				timer.Start();
			}
			else
			{
				if (timer.TimeLeft > 0.1)//在攻击动画时间内(最后一帧前)
				{
					//在攻击动画时间内按下了k键 即为timer添加timeout事件
					if (Input.IsActionJustPressed("Attack1"))
					{
						timer.Timeout += Attack2;

					}
				}
			}


			//timer.Timeout -=Attack2;
			// if (animationPlayer.IsPlaying())
			// {

			// }


		}
		if (this.inputDirection != Vector2.Zero)
		{
			animatedSprite2D.FlipH = (this.inputDirection.X < 0) ? true : false;
			sprite2D.FlipH = (this.inputDirection.X < 0) ? true : false;
		}

		if (Input.IsActionJustPressed("Space"))
		{
			sprite2D.Visible = false;


			animatedSprite2D.Visible = true;
			animationPlayer.Play("Shansuo");
			timer2.Start();
			//timer2.Timeout += OnTimer2Timeout;

		}
		// if (timer2.IsStopped())
		// {
		// 	sprite2D.Visible =true;


		// 	animatedSprite2D.Visible =false;
		// }


		// if (Input.IsActionPressed("ui_left"))
		// {
		// 	velocity.X = -10;
		// }else if (Input.IsActionPressed("ui_right"))
		// {
		// 	velocity.X = 10;
		// }
		//Velocity += velocity;
		Velocity = Velocity.LimitLength(Max_Speed);
		//GD.Print(Velocity);
		MoveAndSlide();
	}
	public void Attack(Vector2 direction)
	{
		if (direction.X >= 0)
		{
			animationPlayer.Play("Attack1");
		}
	}
	public void Attack2()
	{

		animationPlayer.Play("Attack2");
		timer.Timeout -= Attack2;
	}
	private void OnTimer2Timeout()
	{
		Position += inputDirection * 100;
		animationPlayer.Play("Shansuo_Right");
	}

}
