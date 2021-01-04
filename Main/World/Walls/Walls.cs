using Godot;
using System;

public class Walls : Node2D
{
    [Signal]
    public delegate void OnRightWall(Area2D area);

    [Signal]
    public delegate void OnLeftWall(Area2D area);

    public override void _Ready()
    {
        GetNode<Area2D>("Right").Connect("area_entered", this, nameof(OnRight));
        GetNode<Area2D>("Left").Connect("area_entered", this, nameof(OnLeft));
    }

    public void OnRight(Area2D area)
    {
        if (area.GetType().ToString() == "Enemy")
        {
            EmitSignal(nameof(OnRightWall), area);
        }

    }

    public void OnLeft(Area2D area)
    {
        if (area.GetType().ToString() == "Enemy")
        {
            EmitSignal(nameof(OnLeftWall), area);
        }

    }
}
