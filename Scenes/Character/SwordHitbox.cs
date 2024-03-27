using Godot;
using System;

public partial class SwordHitbox : Area2D
{
	private CollisionShape2D collisionShape2D;
	public Stats stats;
	public Vector2 repelVector = Vector2.Zero;

	public override void _Ready()
	{
		stats = GetNode<Stats>("../../Stats");
		collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		//AreaEntered += OnHurtboxEntered;
	}

	// private void OnHurtboxEntered(Area2D area)
	// {
	// 	if ((area as Hurtbox).NeedEffect)
	// 	{

	// 	}

	// }


	// private void CreateHitEffect()
	// {
	// 	var effect = GD.Load<PackedScene>("res://Scenes/Effects/hit_effect.tscn").Instantiate<HitEffect>();
	// 	var main = GetTree().CurrentScene;
	// 	main.AddChild(effect);
	// 	effect.GlobalPosition = collisionShape2D.GlobalPosition;
	// 	GD.Print("已经创建了攻击特效");
	// }
}
