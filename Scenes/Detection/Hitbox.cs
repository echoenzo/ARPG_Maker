using Godot;
using System;

public partial class Hitbox : Area2D
{
    public Stats stats;
    public override void _Ready()
    {
        stats = GetParent().GetNode<Stats>("Stats");

        //AreaEntered += OnAttack;
    }

    // private void OnAttack(Area2D area)
    // {
    //     var playerStats = area.GetParent().GetNode<Stats>("Stats");
    //     playerStats.ChangeHealth(stats.Damage);
    // }

}
