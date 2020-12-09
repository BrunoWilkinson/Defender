using Godot;
using System;

public class Missile : Area2D
{
    [Export]
    public float speed = 200;

    public Vector2 Velocity = new Vector2();

    public override void _Process(float delta)
    {
        Position += Velocity * speed * delta;
    }

}
