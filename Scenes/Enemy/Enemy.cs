using Godot;
using System;

public class Enemy : Area2D
{
    [Signal]
    public delegate void onDestroy();
    [Signal]
    delegate void onShoot(PackedScene bullet, Vector2 location);
    private PackedScene _rock = GD.Load<PackedScene>("res://Scenes/Rock/Rock.tscn");
    public void onHit(Area2D area)
    {
        if (area.GetType().ToString() == "Missile")
        {
            QueueFree();
            area.QueueFree();
            EmitSignal(nameof(onDestroy));
        }
    }
    public void shoot()
    {
        EmitSignal(nameof(onShoot), _rock, GlobalPosition);
    }
    public override void _Ready()
    {
        Connect("area_entered", this, nameof(onHit));
        GetNode<Timer>("RateOfFire").Connect("timeout", this, nameof(shoot));
    }
}
