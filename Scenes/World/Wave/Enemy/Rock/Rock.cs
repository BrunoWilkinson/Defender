using Godot;
using System;

public class Rock : Area2D
{
    [Export]
    public float speed = 200;

    public Vector2 velocity = new Vector2();

    public void OnHitShield(Area2D area)
    {
        if (area.Name == "Shield")
        {
            QueueFree();
        }
    }

    public override void _Ready()
    {
        GetNode<VisibilityNotifier2D>("ScreenCheck").Connect("screen_exited", this, nameof(OnExit));
        Connect("area_entered", this, nameof(OnHitShield));
    }

    public override void _Process(float delta)
    {
        Position += velocity * speed * delta;
    }

    public void OnExit()
    {
        QueueFree();
    }
}
