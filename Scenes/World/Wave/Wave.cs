using Godot;
using System;

public class Wave : Node2D
{
    [Signal]
    delegate void Defeat();

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

    public void OnCollideRightWall(Area2D area)
    {
        xState = MovementState.MOVE_LEFT;
        yState = MovementState.MOVE_DOWN;
    }

    public void OnCollideLeftWall(Area2D area)
    {
        xState = MovementState.MOVE_RIGHT;
        yState = MovementState.MOVE_DOWN;
    }

    public override void _Ready()
    {
        Node2D walls = GetNode<Node2D>("../Walls");
        walls.Connect("rightWall", this, nameof(OnCollideRightWall));
        walls.Connect("leftWall", this, nameof(OnCollideLeftWall));
        xState = MovementState.MOVE_RIGHT;
        yState = MovementState.PASSIVE;
        foreach (Node child in GetChildren())
        {
            if (child.GetType().ToString() == "Enemy")
            {
                child.Connect("onDestroy", this, nameof(speedUp));
            }
        }
    }

    public void speedUp()
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

    public override void _Process(float delta)
    {
        Movement(delta);
        isDefeat();
    }

    private void isDefeat()
    {
        if (GetChildCount() == 0)
        {
            EmitSignal(nameof(Defeat));
        }
    }

    private void Movement(float delta)
    {
        Vector2 screenSize = GetViewport().Size;

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
