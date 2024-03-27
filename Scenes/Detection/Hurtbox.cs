using Godot;
using System;

public abstract partial class Hurtbox : Area2D
{

    public Timer timer;
    /// <summary>
    /// 在hurtbox区域被进入时触发的回调  必须重写
    /// </summary>
    /// <param name="area2D"></param> <summary>
    /// 
    /// </summary>
    /// <param name="area2D"></param>
    public abstract void OnHurtboxAreaEntered(Area2D area2D);
    /// <summary>
    /// 产生特效的虚方法  如果不需要可以不用重写
    /// </summary>
    public virtual void CreateEffect(Vector2 putPosition)
    {

    }

    public override void _Ready()
    {
        AreaEntered += OnHurtboxAreaEntered;
        timer = GetNode<Timer>("Timer");
    }
}
