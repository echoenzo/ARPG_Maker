using Godot;
using System;

public partial class GrassHurtbox : Hurtbox
{
    public event Action Finished;
    public override void OnHurtboxAreaEntered(Area2D area2D)
    {
        CreateEffect(GlobalPosition);
        Finished?.Invoke();
    }
    public override void CreateEffect(Vector2 vector2)
    {
        var grassEffect = GD.Load<PackedScene>("res://Scenes/Effects/grass_effect.tscn").Instantiate<GrassEffect>();//载入场景
                                                                                                                    //var grassEffect = ResourceLoader.Load("res://Scenes/Effects/grass_effect.tscn");
        var main = GetTree().CurrentScene;//获取主场景
        main.AddChild(grassEffect);//在主场景下添加子节点
        grassEffect.GlobalPosition = vector2;//让当前草节点的位置赋值给子节点
                                             //this.Visible = false;


    }

}
