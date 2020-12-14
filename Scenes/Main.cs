using Godot;
using System;

public class Main : Node2D
{
    public override void _Ready()
    {
        GetNode<Area2D>("Player").Connect("PressShoot", this, nameof(OnPlayerShoot));
    }

    public void OnPlayerShoot(PackedScene missile, Vector2 location)
    {
        Missile missileInstance = (Missile)missile.Instance();
        AddChild(missileInstance);
        missileInstance.Position = location;
        missileInstance.Velocity.y = -1;
    }

}
