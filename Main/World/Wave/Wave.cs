using Godot;
using System;

public class Wave : Node2D
{
    [Signal]
    delegate void OnDefeat();

    [Export]
    public float speed = 5;
    [Export]
    public float drop = 5;
    [Export]
    public float speedUpBy = 5;
    enum MovementState
    {
        MOVE_RIGHT,
        MOVE_LEFT,
        MOVE_DOWN,
        PASSIVE,
    }
    private MovementState xState;
    private MovementState yState;
    private Vector2 velocity = Vector2.Zero;

    public override void _Ready()
    {
        xState = MovementState.MOVE_RIGHT;
        yState = MovementState.PASSIVE;
        foreach (Node child in GetChildren())
        {
            if (child is Enemy)
            {
                child.Connect("OnDestroy", this, nameof(SpeedUp));
            }
        }
    }

    public override void _Process(float delta)
    {
        Movement(delta);
        IsDefeat();
    }

    private void IsDefeat()
    {
        if (GetChildCount() == 0)
        {
            EmitSignal(nameof(OnDefeat));
        }
    }

    public void OnCollideRight()
    {
        xState = MovementState.MOVE_LEFT;
        yState = MovementState.MOVE_DOWN;
    }

    public void OnCollideLeft()
    {
        xState = MovementState.MOVE_RIGHT;
        yState = MovementState.MOVE_DOWN;
    }

    public void SpeedUp()
    {
        if (GetChildCount() == 2)
        {
            speed *= 2;
        }
        else
        {
            speed += speedUpBy;
        }
    }

    private void Movement(float delta)
    {
        if (xState == MovementState.MOVE_RIGHT && yState == MovementState.PASSIVE)
        {
            velocity = Vector2.Zero;
            velocity.x += 1;
        }
        else if (xState == MovementState.MOVE_LEFT && yState == MovementState.PASSIVE)
        {
            velocity = Vector2.Zero;
            velocity.x -= 1;
        }
        else if (yState == MovementState.MOVE_DOWN)
        {
            Position += new Vector2(0, drop);
            yState = MovementState.PASSIVE;
        }
        else
        {
            velocity = Vector2.Zero;
        }

        Position += velocity.Normalized() * speed * delta;
    }
}
