using Godot;
using System;

public partial class WorldCamera : Camera2D
{
	private TileMap tileMapCliff;

	public override void _Ready()
	{
		tileMapCliff = GetParent().GetNode<TileMap>("CliffTileMap");
		LimitTop = tileMapCliff.GetUsedRect().Position.Y;
		LimitLeft = tileMapCliff.GetUsedRect().Position.X;
		LimitRight = tileMapCliff.GetUsedRect().Size.X * 32;
		LimitBottom = tileMapCliff.GetUsedRect().Size.Y * 32;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
