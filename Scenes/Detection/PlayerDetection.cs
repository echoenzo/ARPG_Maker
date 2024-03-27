using Godot;
using System;

public partial class PlayerDetection : Area2D
{
    public Node2D Player = null;
    //public event Action<Node2D> PlayerEntered;
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }
    public void OnBodyEntered(Node2D body)
    {
        Player = body;
    }
    public void OnBodyExited(Node2D body)
    {
        Player = null;
    }
    public bool CanSeePlayer()
    {
        return Player != null;
    }
}
