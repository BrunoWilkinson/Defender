using Godot;
using System;

public class Enemy : Area2D
{
    [Signal]
    public delegate void OnDestroy();
    [Signal]
    delegate void OnShoot(PackedScene bullet, Vector2 location, Area2D enemy);

    [Export]
    public int enemyVersion = 1;
    private PackedScene _rock = GD.Load<PackedScene>("res://Main/World/Wave/Enemy/Rock/Rock.tscn");

    public override void _Ready()
    {
        Connect("area_entered", this, nameof(OnHit));
        GetNode<Timer>("FireRate").Connect("timeout", this, nameof(Shoot));
        GetNode<AnimatedSprite>("AnimatedSprite").Play("enemy_" + enemyVersion);
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
