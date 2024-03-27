using Godot;
using System;

public partial class BatHurtbox : Hurtbox
{
    public event Action<Area2D> Attacked;
    public override void OnHurtboxAreaEntered(Area2D area2D)
    {
        CreateEffect(GlobalPosition);
        //通知被攻击了  
        Attacked?.Invoke(area2D);
    }
    public override void CreateEffect(Vector2 vector2)
    {
        var effect = GD.Load<PackedScene>("res://Scenes/Effects/hit_effect.tscn").Instantiate<HitEffect>();
        var main = GetTree().CurrentScene;
        main.AddChild(effect);
        effect.GlobalPosition = vector2;
        //GD.Print("已经创建了攻击特效");
    }

}
