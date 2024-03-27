using Godot;
using System;

[GlobalClass]
public partial class PropertyResource : Resource
{
    [ExportCategory("基础属性")]
    [Export]
    public int Health;
    [Export]
    public int MaxHealth;
    [Export]
    public int Damage;
}
