using Godot;
using System;

public class Enemy : Area2D
{
    [Signal]
    public delegate void OnDestroy();
    [Signal]
    delegate void OnShoot(PackedScene bullet, Vector2 location, Area2D enemy);
    private PackedScene _rock = GD.Load<PackedScene>("res://Main/World/Wave/Enemy/Rock/Rock.tscn");

    public override void _Ready()
    {
        Connect("area_entered", this, nameof(OnHit));
        GetNode<Timer>("RateOfFire").Connect("timeout", this, nameof(Shoot));
    }
    public void OnHit(Area2D area)
    {
        if (area is Missile)
        {
            QueueFree();
            area.QueueFree();
            EmitSignal(nameof(OnDestroy));
        }
    }
    public void Shoot()
    {
        EmitSignal(nameof(OnShoot), _rock, GlobalPosition, this);
    }
}
