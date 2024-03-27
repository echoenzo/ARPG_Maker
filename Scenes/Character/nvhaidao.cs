using Godot;
using System;

public partial class nvhaidao : CharacterBody2D
{
	private Timer timer;
	private bool IsSpace = false;
	private float Speed = 4000;
	private Vector2 input;
	private Sprite2D sprite2D;
	private Sprite2D shadow;//阴影
	private AnimationPlayer shadowplayer;//阴影动画
	private AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		shadow = GetNode<Sprite2D>("Shadow");
		shadowplayer = GetNode<AnimationPlayer>("Shadow/AnimationPlayer");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		timer = GetNode<Timer>("Timer");
		timer.Timeout += () =>
		{
			IsSpace = false;
			Speed = 4000;
		};
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDirection = Vector2.Zero;
		inputDirection.X = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		inputDirection.Y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		inputDirection = inputDirection.Normalized();
		if (inputDirection != Vector2.Zero)
		{
			input = inputDirection;
		}
		if (!IsSpace)//没有在闪避才可以控制人物跑动动画播放
		{
			if (inputDirection != Vector2.Zero)
			{

				if (inputDirection.X != 0)
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
				else if (inputDirection.Y != 0)
				{
					if(inputDirection.Y > 0){
						animationPlayer.Play("Down_Run");
					}else
					{
						animationPlayer.Play("Up_Run");
					}
					
				}

			}
			else if (inputDirection == Vector2.Zero)
			{
				if (input.X!=0)
				{
					animationPlayer.Play("Idle");
				}else if (input.Y>0)
				{
					animationPlayer.Play("Down_Idle");
				}else
				{
					animationPlayer.Play("Up_Idle");
				}
				
				
			}
			
		}
		if (!shadowplayer.IsPlaying())
		{
			shadow.Visible = false;
			shadow.Position = Vector2.Zero;
		}

		if (Input.IsActionJustPressed("Space"))
		{
			IsSpace = true;
			timer.Start();
			shadow.Position-=input*10;
			GD.Print(shadow.Position);
			GD.Print(Position);
			shadow.Visible=true;
			if (input.X!=0)
			{
				animationPlayer.Play("Flashing");
				if (input.X > 0)
				{
					shadow.FlipH= true;
				}else
				{
					shadow.FlipH= false;
				}
				shadowplayer.Play("Shadow");
			}else if(input.Y>0)
			{
				animationPlayer.Play("Down_Flashing");
				shadowplayer.Play("Down_Shadow");
			}else
			{
				animationPlayer.Play("Up_Flashing");
				shadowplayer.Play("Up_Shadow");
			}
			
			Speed = 10000;
			//Velocity =inputDirection*100;

		}

		Velocity = (IsSpace ? input : inputDirection) * Speed * (float)delta;

		MoveAndSlide();
	}
}


