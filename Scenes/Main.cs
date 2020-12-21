using Godot;
using System;

public class Main : Node2D
{
    private Node2D _wave;

    private CanvasLayer _hud;

    public int score;
    public int highScore;

    public bool startGame;

    public bool inGame;

    public override void _Ready()
    {
        CreateConnection();
        HandleHud();
        base._Ready();
    }

    public void CreateConnection()
    {
        GetNode<Area2D>("Player").Connect("PressShoot", this, nameof(OnPlayerShoot));
        _wave = GetNode<Node2D>("Wave");
        foreach (Node child in _wave.GetChildren())
        {
            if (child.GetType().ToString() == "Enemy")
            {
                child.Connect("onShoot", this, nameof(OnEnemyShoot));
            }
        }
    }

    public void OnPlayerShoot(PackedScene missile, Vector2 location)
    {
        Missile missileInstance = (Missile)missile.Instance();
        AddChild(missileInstance);
        missileInstance.Position = location;
        missileInstance.Velocity.y = -1;
    }

    public void OnEnemyShoot(PackedScene rock, Vector2 location, Area2D enemy)
    {
        uint randomEnemy = (uint)Math.Ceiling(GD.RandRange(0, _wave.GetChildCount() - 1));
        if (enemy.GetIndex() == randomEnemy)
        {
            Rock rockInstance = (Rock)rock.Instance();
            AddChild(rockInstance);
            rockInstance.Position = location;
            rockInstance.Velocity.y = 1;
        }
    }

    public void HandleHud()
    {
        _hud = GetNode<CanvasLayer>("HUD");
        if (startGame)
        {
            HUD.StartGame();
        }
        else if (inGame)
        {
            HUD.InGame();
        }
    }
}
