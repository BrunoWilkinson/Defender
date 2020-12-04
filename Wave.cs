using Godot;
using System;

public class Wave : Area2D
{
    [Export]
    public float speed = 40;

    [Export]
    public PackedScene enemyScene;

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
        GenerateEnemies(9, 5, 128, 192);
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

    private void GenerateEnemies(int x, int y, int xOffset, int yOffset) {
        Vector2 screenSize = GetViewportRect().Size;
        Vector2 waveSize = new Vector2(screenSize.x - xOffset, screenSize.y - yOffset);
        
        for(int rows = 0; y > rows; rows++) {
            for(int columns = 0; x > columns; columns++) {
                var enemy = (Enemy)enemyScene.Instance();
                enemy.Name = "Enemy" + rows + columns;
                AddChild(enemy);
            }
        }
        
    }
}
