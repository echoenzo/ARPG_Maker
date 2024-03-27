using Godot;
using System;

public partial class Grass : Node2D
{
	private GrassHurtbox hurtbox;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hurtbox = GetNode<GrassHurtbox>("Hurtbox");
		hurtbox.Finished += QueueFree;

		// hurtbox.AreaEntered += (a) =>
		// {
		// 	CreateGrassEffect();
		// 	GD.Print("造成伤害");
		// };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public override void _PhysicsProcess(double delta)
	{
		// if (Input.IsActionJustPressed("Attack1"))
		// {


		// }
	}
	/// <summary>
	/// 创建草的特效
	/// </summary> <summary>
	/// 
	/// </summary>
	// private void CreateGrassEffect()
	// {
	// 	var grassEffect = GD.Load<PackedScene>("res://Scenes/Effects/grass_effect.tscn").Instantiate<GrassEffect>();//载入场景
	// 																												//var grassEffect = ResourceLoader.Load("res://Scenes/Effects/grass_effect.tscn");
	// 	var main = GetTree().CurrentScene;//获取主场景
	// 	main.AddChild(grassEffect);//在主场景下添加子节点
	// 	grassEffect.GlobalPosition = GlobalPosition;//让当前草节点的位置赋值给子节点
	// 												//this.Visible = false;
	// 	QueueFree();
	// }
}
