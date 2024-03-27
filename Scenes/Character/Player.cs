using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody2D
{
	[Export]
	int Max_Speed = 100;
	[Export]
	int acceleration = 600;//加速度
	[Export]
	int friction = 1200;//摩擦力
	private AnimationPlayer animationPlayer;
	private AnimationPlayer blinkAnimationPlayer;
	private AnimationTree animationTree;
	private AnimationNodeStateMachinePlayback animationNodeState;
	private SwordHitbox hitbox;
	private State State = State.MOVE;
	private Vector2 rollVector = Vector2.Left;//闪避方向
	private Stats stats;
	private PlayerHurtbox playerHurtbox;
	private PackedScene playerHurtSound;

	public override void _Ready()
	{
		hitbox = GetNode<SwordHitbox>("HitboxPivot/Hitbox");
		playerHurtbox = GetNode<PlayerHurtbox>("Hurtbox");

		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		blinkAnimationPlayer = GetNode<AnimationPlayer>("BlinkAnimation");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;//激活动画树
		animationNodeState = animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();//变体的转换需要使用as和from
		stats = GetNode<Stats>("Stats");
		stats.OnDeathEvent += Dead;
		playerHurtbox.Hurted += stats.ChangeHealth;
		playerHurtSound = GD.Load<PackedScene>("res://Scenes/Character/PlayerHurtSound.tscn");
		GD.Randomize();//改变每次进入游戏的随机种子 避免每次随机都是同样的结果
		playerHurtbox.InvincibilityStarted += () =>
		{
			blinkAnimationPlayer.Play("Start");
		};
		playerHurtbox.InvincibilityEnded += () =>
		{
			blinkAnimationPlayer.Play("End");
		};

	}


	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		switch (State)
		{

			case State.MOVE:
				MoveState(delta);
				break;
			case State.ATTACK:
				AttackState();
				break;
			case State.ROLL:
				RollState();
				break;
			case State.DEAD:
				Dead();
				break;
		}
	}
	private void MoveState(double delta)
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
			// if (inputDirection.X>0)
			// {
			// 	animationPlayer.Play("Run_Right");
			// }else
			// {
			// 	animationPlayer.Play("Run_Left");
			// }
			//如果方向值不为0时  设置animationtree的Idle的值为方向值  这样在方向值为0时 animationtree的idle值不会改变
			rollVector = inputDirection;//在移动方向不为0的情况下 给闪避方向赋值
			hitbox.repelVector = inputDirection;
			animationTree.Set("parameters/Idle/blend_position", inputDirection);
			animationTree.Set("parameters/Run/blend_position", inputDirection);
			animationTree.Set("parameters/Attack/blend_position", inputDirection);
			animationTree.Set("parameters/Roll/blend_position", inputDirection);

			//GD.Print(inputDirection);
			animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>().Travel("Run");
			//animationNodeState.Travel("Run");
			Velocity = Velocity.MoveToward(inputDirection * Max_Speed, acceleration * (float)delta);

		}
		else
		{
			//animationPlayer.Play("Idle_Right");
			//animationNodeState.Travel("Idle");
			animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>().Travel("Idle");
			Velocity = Velocity.MoveToward(Vector2.Zero, friction * (float)delta);
		}
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
		Move();
		if (Input.IsActionJustPressed("Attack1"))
		{
			State = State.ATTACK;
		}
		if (Input.IsActionJustPressed("Space"))
		{
			State = State.ROLL;
		}
	}
	private void AttackState()
	{
		Velocity = Vector2.Zero;
		animationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>().Travel("Attack");
	}
	private void RollState()
	{
		animationNodeState.Travel("Roll");
		Velocity = Max_Speed * rollVector;
		Move();
	}
	public void AnimationFinished()
	{
		State = State.MOVE;
	}
	public void RollAnimationFinished()
	{
		Velocity = Vector2.Zero;
		State = State.MOVE;
	}
	private void Move()
	{
		MoveAndSlide();
	}
	private void Dead()
	{
		//死亡前 实例化死亡音效
		var playerHurtSound = this.playerHurtSound.Instantiate<AudioStreamPlayer>();
		GetTree().CurrentScene.AddChild(playerHurtSound);
		QueueFree();
	}

}
