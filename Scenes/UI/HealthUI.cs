using Godot;
using System;

public partial class HealthUI : Control
{


	private Stats stats;
	private int health;
	private int maxHealth;
	private Label healthLabel;
	public int Health { get => health; set => health = value; }
	public int MaxHealth { get => maxHealth; set => maxHealth = value; }


	public override void _Ready()
	{
		stats = GetTree().CurrentScene.GetNode<Stats>("YSort/CharacterBody2D/Stats");
		healthLabel = GetNode<Label>("Label");
		//healthLabel.Text = $"生命值:{stats.Health}";
		stats.OnHealthChange += (h) =>
		{
			healthLabel.Text = $"生命值:{h}";
		};
		healthLabel.Text = $"生命值:{stats.Health}";
	}


	public override void _Process(double delta)
	{

	}
}
