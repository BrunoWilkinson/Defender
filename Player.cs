using Godot;
using System;

public class Player : Area2D
{
    [Export]
    public float speed = 1000;

    [Export]
    public float spriteSize = 32;

    [Export]
    public PackedScene missile;

    public Vector2 screenSize;

    public override void _Ready()
    {
        screenSize = GetViewport().Size;
    }

    public override void _Process(float delta)
    {
        Controls(delta);
    }

    private void Controls(float delta)
    {
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += 1;
        }
        if (Input.IsActionPressed("move_left"))
        {
            velocity.x -= 1;
        }

        Position += velocity * speed * delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, spriteSize, screenSize.x - spriteSize),
            y: Position.y
        );
    }

    private void Shoot(float delta)
    {

    }
}
