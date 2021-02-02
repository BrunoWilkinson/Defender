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

    private AnimatedSprite _anim;

    public override void _Ready()
    {
        Connect("area_entered", this, nameof(OnHit));
        GetNode<Timer>("FireRate").Connect("timeout", this, nameof(Shoot));
        _anim = GetNode<AnimatedSprite>("AnimatedSprite");
        _anim.Play("enemy_" + enemyVersion);
        _anim.Connect("animation_finished", this, nameof(OnAnimationFinished));
    }

    public void OnAnimationFinished()
    {
        if (_anim.Animation == "death")
        {
            QueueFree();
        }
    }

    public void OnHit(Area2D area)
    {
        if (area is Missile)
        {
            area.QueueFree();
            _anim.Play("death");
            GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
            EmitSignal(nameof(OnDestroy));
        }
    }
    public void Shoot()
    {
        EmitSignal(nameof(OnShoot), _rock, GlobalPosition, this);
    }
}
