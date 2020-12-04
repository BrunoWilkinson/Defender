using Godot;
using System;

public class Wave : Area2D
{
    [Export]
    private float speed = 40;

    enum AIState
    {
        MOVE_RIGHT,
        MOVE_LEFT
    }

    private Vector2 screenSize;
    private AnimatedSprite animatedSprite;
    private Vector2 startPosition;
    private AIState currentState;
    private Vector2 velocity = Vector2.Zero;

    public void OnCollideRight()
    {
        GD.Print("COLLIDE WITH RIGHT WALL");
        currentState = AIState.MOVE_LEFT;
        velocity = new Vector2(0, 1);
    }

     public void OnCollideLeft()
    {
        GD.Print("COLLIDE WITH LEFT WALL");
        currentState = AIState.MOVE_RIGHT;
        velocity = new Vector2(0, 1);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        currentState = AIState.MOVE_RIGHT;
        // animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    //Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Movement(delta);
    }

    private void Movement(float delta)
    {
        if (currentState == AIState.MOVE_RIGHT) {
            velocity.x += 1;
        } else {
            velocity.x -= 1;
        }

        Position += velocity.Normalized() * speed * delta;
    }
}
