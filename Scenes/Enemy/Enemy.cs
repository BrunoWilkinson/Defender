using Godot;
using System;

public class Enemy : Area2D
{
    public override void _Ready()
    {
        Connect("area_entered", this, nameof(onHit));
    }
    public void onHit(Area2D area)
    {
        if (area.GetType().ToString() == "Missile")
        {
            QueueFree();
            area.QueueFree();
        }
    }
}
