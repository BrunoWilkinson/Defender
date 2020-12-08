using Godot;
using System;

public class Missile : Area2D
{
    [Export]
    public float speed = 200;
    [Export]
    public Boolean up = false;

    public override void _Process(float delta)
    {
        Vector2 velocity = new Vector2(0, 1);
        if(up) {
            velocity.y = -1;
        }
        Position += velocity * speed * delta;
    }

}
