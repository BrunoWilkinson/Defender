using Godot;
using System;

public class Walls : Node2D
{
    [Signal]
    public delegate void OnRightCollision();

    [Signal]
    public delegate void OnLeftCollision();

    private Vector2 _startPos;

    public override void _Ready()
    {
        _startPos = Position;
        GetNode<Area2D>("Right").Connect("area_entered", this, nameof(OnRight));
        GetNode<Area2D>("Left").Connect("area_entered", this, nameof(OnLeft));
    }

    public void OnRight(Area2D area)
    {
        if (area is Enemy)
        {
            EmitSignal(nameof(OnRightCollision));
        }
    }

    public void OnLeft(Area2D area)
    {
        if (area is Enemy)
        {
            EmitSignal(nameof(OnLeftCollision));
        }

    }
}
