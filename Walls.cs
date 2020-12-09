using Godot;
using System;

public class Walls : Node2D
{
    [Signal]
    public delegate void rightWall();

    [Signal]
    public delegate void leftWall();

    public void OnRight(Area2D area)
    {
        EmitSignal(nameof(rightWall));
    }

    public void OnLeft(Area2D area)
    {
        EmitSignal(nameof(leftWall));
    }

    public override void _Ready()
    {
        GetNode<Area2D>("Right").Connect("area_entered", this, nameof(OnRight));
        GetNode<Area2D>("Left").Connect("area_entered", this, nameof(OnLeft));    
    }

}
