using Godot;
using System;
using System.Collections.Generic;

public partial class Bat : CharacterBody2D
{
	private BatHurtbox hurtbox;
	private PlayerDetection playerDetection;
	private AnimatedSprite2D sprite2D;
	private Stats stats;
	private EnemyState enemyState = EnemyState.IDLE;//敌人状态
													//private Vector2 repelVector = Vector2.Zero;
	private SoftCollision softCollision;
	private WanderController wanderController;
	private List<EnemyState> randomStateList = new List<EnemyState>() { EnemyState.IDLE, EnemyState.WANDER };

	[Export]
	public int Friction;
	[Export]
	public int Max_Speed;
	[Export]
	public int Acceleration;
	public override void _Ready()
	{
		hurtbox = GetNode<BatHurtbox>("Hurtbox");
		hurtbox.Attacked += OnBatHurtboxEntered;//被攻击后做什么
		playerDetection = GetNode<PlayerDetection>("PlayerDetectionArea");
		sprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		stats = GetNode<Stats>("Stats");
		softCollision = GetNode<SoftCollision>("SoftCollision");
		wanderController = GetNode<WanderController>("WanderController");
		//连接蝙蝠死亡信号
		//stats.OnDeath += Death;
		stats.OnDeathEvent += DeathEvent;
		PickNewState(randomStateList);
	}

	private void DeathEvent()
	{
		CreateDeathEffect();
		//Test();
		//GD.Print("蝙蝠死亡 使用event调用");
		QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		switch (enemyState)
		{
			case EnemyState.IDLE:
				{
					//idle状态下  速度会慢慢趋于0 停下来

					Velocity = Velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
					//检测玩家是否进入追逐范围
					SeePlayer();
					//timer超时后可以随机切换状态 idle wander
					if (wanderController.GetTimeLeft() <= 0)
					{
						PickNewState(randomStateList);
						wanderController.StartTimer(new Random().Next(5, 11));//每5-10s切换一次状态
					}
					break;
				}
			case EnemyState.WANDER:
				{
					SeePlayer();
					//timer超时后可以随机切换状态 idle wander
					if (wanderController.GetTimeLeft() <= 0)
					{
						PickNewState(randomStateList);
						wanderController.StartTimer(10);//每10s切换一次状态
					}
					//wander中需要移动到targetposition
					Vector2 direction = GlobalPosition.DirectionTo(wanderController.TargetPosition);
					Velocity = Velocity.MoveToward(direction * Max_Speed * 2 / 3, Acceleration * (float)delta);//游荡速度减少
					if (GlobalPosition.DistanceTo(wanderController.TargetPosition) <= 4)
					{
						enemyState = EnemyState.IDLE;
					}
					break;
				}
			case EnemyState.CHASE:
				{
					//追逐玩家 朝玩家移动
					var player = playerDetection.Player;
					if (player != null)
					{
						//Vector2 direction = (player.GlobalPosition - hurtbox.GlobalPosition).Normalized();
						Vector2 direction = hurtbox.GlobalPosition.DirectionTo(player.GlobalPosition);
						Velocity = Velocity.MoveToward(direction * Max_Speed, Acceleration * (float)delta);
						MoveAndSlide();
					}
					else
					{
						enemyState = EnemyState.IDLE;
					}

					break;
				}
		}
		sprite2D.FlipH = Velocity.X < 0;
		MoveSoftCollision(delta);

		MoveAndSlide();
	}
	/// <summary>
	/// 蝙蝠被攻击时
	/// </summary>
	/// <param name="area2D"></param>
	private void OnBatHurtboxEntered(Area2D area2D)
	{
		//从上调用下 应该使用方法 
		//从下到上 应该使用信号

		//收到伤害
		// stats.Health -= 1;
		// GD.Print(stats.Health);
		stats.ChangeHealth((area2D as SwordHitbox).stats.Damage);//用玩家的攻击力来改变蝙蝠的血量
																 // if (stats.Health <= 0)
																 // {
																 // 	QueueFree();
																 // }

		//被击退
		var repelVector = (area2D as SwordHitbox).repelVector * 30;
		Velocity = repelVector;//每碰撞一次 赋值一次速度

		//QueueFree();
	}
	// private void Death()
	// {
	// 	// GD.Print("Death");
	// 	// QueueFree();
	// }
	/// <summary>
	/// 创建死亡特效
	/// </summary>
	private void CreateDeathEffect()
	{
		var batDeathEffect = GD.Load<PackedScene>("res://Scenes/Effects/enemy_death_effect.tscn").Instantiate<GrassEffect>();
		var main = GetParent();//ysort 放在这里面的原因是希望爆炸也能带有ysort排序功能
		main.AddChild(batDeathEffect);
		batDeathEffect.GlobalPosition = GlobalPosition;
	}
	/// <summary>
	/// 随机idle和wander状态
	/// </summary>
	/// <param name="enemyStates"></param>
	private void PickNewState(List<EnemyState> enemyStates)
	{
		Random random = new Random();
		enemyState = enemyStates[random.Next(0, 2)];
	}

	public void SeePlayer()
	{
		if (playerDetection.CanSeePlayer())
		{
			enemyState = EnemyState.CHASE;
		}
	}
	/// <summary>
	/// 移动软碰撞 避免怪物堆在一起
	/// </summary>
	/// <param name="delta"></param>
	public void MoveSoftCollision(double delta)
	{
		if (softCollision.IsCollisioning())
		{
			Velocity += softCollision.GetPutVector() * (float)delta * 400;
		}
	}

}
