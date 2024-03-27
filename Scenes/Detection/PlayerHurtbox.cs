using Godot;
using System;

public partial class PlayerHurtbox : Hurtbox
{
    private Stats stats;
    private bool invincible = false;
    public bool Invincible
    {
        get => invincible;
        set
        {
            invincible = value;
            if (invincible)
            {
                InvincibilityStarted?.Invoke();
            }
            else
            {
                InvincibilityEnded?.Invoke();
            }
        }
    }
    public event Action InvincibilityStarted;
    public event Action InvincibilityEnded;
    public event Action<int> Hurted;//受伤
    public override void _Ready()
    {
        base._Ready();
        stats = GetParent().GetNode<Stats>("Stats");
        timer.Timeout += () =>
        {
            //Invincible = false;
            //Monitoring = true;
            SetDeferred(PropertyName.Monitoring, true);
            Invincible = false;
            stats.CanDamage = true;
            GD.Print(Monitoring);
        };
        //AreaEntered += OnHurtboxAreaEntered;
    }
    private int n = 1;
    public override void OnHurtboxAreaEntered(Area2D area2D)
    {
        StartInvincibleTimer(2f);
        //产生被攻击的特效 
        CreateEffect(GlobalPosition);
        //玩家的伤害盒子被进入  检测受伤
        Hurted?.Invoke((area2D as Hitbox).stats.Damage);
        stats.CanDamage = false;//开始无敌后不能收到伤害
        GD.Print($"被击中{n++}次");

    }
    /// <summary>
    /// 开始无敌时间
    /// </summary>
    /// <param name="time"></param>
    private void StartInvincibleTimer(float time)
    {
        //Invincible = true;
        timer.Start(time);
        GD.Print("开始无敌");
        Invincible = true;
        SetDeferred(PropertyName.Monitoring, false);
        //Monitoring = false;
        GD.Print(Monitoring);
    }
    public override void CreateEffect(Vector2 putPosition)
    {
        var effect = GD.Load<PackedScene>("res://Scenes/Effects/hit_effect.tscn").Instantiate<HitEffect>();
        var main = GetTree().CurrentScene;
        main.AddChild(effect);
        effect.GlobalPosition = putPosition;

    }
}
