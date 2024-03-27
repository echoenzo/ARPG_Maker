using Godot;
using System;
using static Godot.GD;
/// <summary>
/// 人物的状态属性 包含生命值 防御力等
/// </summary> <summary>
/// 
/// </summary>
public partial class Stats : Node
{
	private PropertyResource resource;
	/// <summary>
	/// 是否能收到伤害
	/// </summary> <summary>
	/// 
	/// </summary>
	public bool CanDamage = true;
	[Signal]
	public delegate void OnDeathEventHandler();
	/// <summary>
	/// 最好使用event代替信号使用
	/// </summary>
	public event Action OnDeathEvent;
	public event Action<int> OnHealthChange;
	[Export]
	/// <summary>
	/// 最大血量
	/// </summary>
	/// <value></value>
	public int MaxHealth { get; set; }

	private int health;
	/// <summary>
	/// 生命值
	/// </summary>
	public int Health
	{
		get { return health; }
		set
		{
			if (CanDamage)
			{
				health = value;
				OnHealthChange?.Invoke(health);
				//GD.Print("Health: " + health);
			}

			if (health <= 0)
			{
				//发出死亡信号
				//EmitSignal(SignalName.OnDeath);
				OnDeathEvent?.Invoke();//效果类似信号 在死亡时调用
			}
		}
	}
	[Export]
	public int Damage { get; set; }
	public void ChangeHealth(int attack)
	{
		Health -= attack;
		GD.Print(Health);
	}
	public override void _Ready()
	{
		Health = MaxHealth;

	}

	public override void _EnterTree()
	{
		base._EnterTree();
		resource = ResourceLoader.Load<PropertyResource>("res://Common/PropertyEntity.tres");
		// GD.Print(resource.Health);
		// GD.Print(resource.MaxHealth);
		// GD.Print(resource.Damage); //测试成功
	}



}
