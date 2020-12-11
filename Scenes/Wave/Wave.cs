using Godot;
using System;

public class Wave : Node2D
{
  [Export]
  public float speed = 40;

  enum MovementState
  {
    MOVE_RIGHT,
    MOVE_LEFT,
    MOVE_DOWN,
    PASSIVE,
  }

  private AnimatedSprite animatedSprite;
  private MovementState xState;
  private MovementState yState;
  private Vector2 velocity = Vector2.Zero;

  public void OnCollideRightWall(Area2D area)
  {
    GD.Print("AREA NAME:", area.Name, "\n");
    GD.Print("Before X:", xState, " Y:", yState, "\n");
    xState = MovementState.MOVE_LEFT;
    yState = MovementState.MOVE_DOWN;
    GD.Print("After X:", xState, " Y:", yState, "\n");
    GD.Print("--------------------------------------------\n");
  }

  public void OnCollideLeftWall(Area2D area)
  {
    GD.Print("AREA NAME:", area.Name, "\n");
    GD.Print("Before X:", xState, " Y:", yState, "\n");
    xState = MovementState.MOVE_RIGHT;
    yState = MovementState.MOVE_DOWN;
    GD.Print("After X:", xState, " Y:", yState, "\n");
    GD.Print("--------------------------------------------\n");
  }

  public override void _Ready()
  {
    Node2D walls = GetNode<Node2D>("../Walls");
    walls.Connect("rightWall", this, nameof(OnCollideRightWall));
    walls.Connect("leftWall", this, nameof(OnCollideLeftWall));
    xState = MovementState.MOVE_RIGHT;
    yState = MovementState.PASSIVE;
    // animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
  }

  public override void _Process(float delta)
  {
    Movement(delta);
  }

  private void Movement(float delta)
  {
    Vector2 screenSize = GetViewport().Size;

    if (xState == MovementState.MOVE_RIGHT && yState == MovementState.PASSIVE)
    {
      velocity.x += 1;
    }
    else if (xState == MovementState.MOVE_LEFT && yState == MovementState.PASSIVE)
    {
      velocity.x -= 1;
    }
    else if (yState == MovementState.MOVE_DOWN)
    {
      velocity.y = 1;
      yState = MovementState.PASSIVE;
    }
    else
    {
      velocity = Vector2.Zero;
    }

    Position += velocity.Normalized() * speed * delta;
     Position = new Vector2(
        x: Mathf.Clamp(Position.x, Position.x - 64, screenSize.x - 64),
        y: Position.y
    );
  }
}
