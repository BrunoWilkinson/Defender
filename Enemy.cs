using Godot;
using System;

public class Enemy : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    private float speed = 40;

    enum AIState {
        MOVE_RIGHT,
        MOVE_LEFT
    }

    private Vector2 screenSize;
    private AnimatedSprite animatedSprite;
    private Vector2 startPosition;

    private AIState currentState;

    private void Movement(float delta)
    {
        float xMax = screenSize.x - startPosition.x;
        float yMax = screenSize.y - startPosition.y;
        Vector2 velocity = Vector2.Zero;

        if(Position.x == startPosition.x && currentState != AIState.MOVE_RIGHT) {
            currentState = AIState.MOVE_RIGHT;
            velocity.y += startPosition.y;
        } else if (Position.x == xMax) {
            currentState = AIState.MOVE_LEFT;
            velocity.y += startPosition.y;
        }

        if (currentState == AIState.MOVE_RIGHT)
        {
            velocity.x += 1;
        } else if (currentState == AIState.MOVE_LEFT) {
            velocity.x -= 1;
        }
        
        Position += velocity * speed * delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, startPosition.x, xMax),
            y: Mathf.Clamp(Position.y, startPosition.y, yMax)
        );
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        screenSize = GetViewportRect().Size;
        startPosition = Position;
        currentState = AIState.MOVE_RIGHT;
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    //Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Movement(delta);
    }
}
