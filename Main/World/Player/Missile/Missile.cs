using Godot;
using System;

public class Missile : Area2D
{
    [Export]
    public float speed = 200;

    public Vector2 velocity = new Vector2();

    public override void _Ready()
    {
        GetNode<VisibilityNotifier2D>("ScreenCheck").Connect("screen_exited", this, nameof(OnExit));
    }

    public override void _Process(float delta)
    {
        velocity.y = -1;
        Position += velocity * speed * delta;
        GD.Print(Position);
    }

    public void OnExit()
    {
        QueueFree();
    }

}
