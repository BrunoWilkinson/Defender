using Godot;
using System;

public class Wave : Node2D
{
    [Export]
    public float speed = 40;

    [Export]
    public PackedScene enemyScene;
    [Export]
    public int row = 5;
    [Export]
    public int column = 8;

    [Export]
    public int rowOffset = 192;
    [Export]
    public int columnOffset = 128;
    [Export]
    public int gap = 16;

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

    public void OnCollideWall(Area2D area)
    {
        GD.Print("COLLIDE WITH RIGHT WALL");
        GD.Print(area.Name);
        if (area.Name == "Right") {
            currentState = AIState.MOVE_LEFT;
        } else if (area.Name == "Left") {
            currentState = AIState.MOVE_RIGHT;
        }
        velocity = new Vector2(0, 1);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GenerateEnemies(row, column, rowOffset, columnOffset);
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
        if (currentState == AIState.MOVE_RIGHT)
        {
            velocity.x += 1;
        }
        else
        {
            velocity.x -= 1;
        }

        Position += velocity.Normalized() * speed * delta;
    }

    private void GenerateEnemies(int rows, int columns, int yOffset, int xOffset)
    {
        Vector2 screenSize = GetViewportRect().Size;
        Vector2 waveSize = new Vector2(screenSize.x - xOffset, screenSize.y - yOffset);
        int counter = 0;

        for (int y = 1; rows >= y; y++)
        {
            for (int x = 1; columns >= x; x++)
            {
                string enemyName = "Enemy";
                AddChild((Enemy)enemyScene.Instance(), true);
                counter++;
                if (counter > 1) {
                    enemyName += counter;
                }
                Area2D enemyNode = GetNode<Area2D>(enemyName);
                enemyNode.Connect("area_entered", this, nameof(OnCollideWall));
                enemyNode.Position = new Vector2(gap * x, gap * y);
                GD.Print(enemyNode.Position);
            }
        }
    }
}
