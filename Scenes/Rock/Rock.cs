using Godot;
using System;

public class Rock : Area2D
{
    [Export]
    public float speed = 200;

    public Vector2 Velocity = new Vector2();

    public void onHitShield(Area2D area)
    {
        if (area.Name == "Shield")
        {
            QueueFree();
        }
    }

    public override void _Ready()
    {
        GetNode<VisibilityNotifier2D>("ScreenCheck").Connect("screen_exited", this, nameof(onExit));
        Connect("area_entered", this, nameof(onHitShield));
    }

    public override void _Process(float delta)
    {
        Position += Velocity * speed * delta;
    }

    public void onExit()
    {
        QueueFree();
    }
}
