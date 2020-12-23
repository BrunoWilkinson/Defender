using Godot;
using System;

public class Main : Node2D
{
    public int score;
    public int highScore;

    public bool startGame;

    public bool inGame;

    private PackedScene _waveScene = GD.Load<PackedScene>("res://Scenes/Wave/Wave.tscn");

    private PackedScene _playerScene = GD.Load<PackedScene>("res://Scenes/Player/Player.tscn");

    private Timer _waveTimer;

    public override void _Ready()
    {
        GetNode<CanvasLayer>("HUD").Connect("NewGame", this, nameof(StartGame));
        _waveTimer = GetNode<Timer>("WaveTimer");
        _waveTimer.Connect("timeout", this, nameof(UnPause));
        InMenu();
    }

    public void InMenu()
    {
        HUD.MenuGame();
    }

    public void StartGame()
    {
        _waveTimer.Start();
        HUD.InGame();
        AddChild(_waveScene.Instance());
        AddChild(_playerScene.Instance());
        CreateConnection();
        GetTree().Paused = true;
    }

    public void UnPause()
    {
        GetTree().Paused = false;
    }

    public void ClearChildren()
    {
        foreach (Node child in GetChildren())
        {
            if (child is Missile || child is Rock || child is Player || child is Wave)
            {
                RemoveChild(child);
            }
        }
    }

    public void GameOver()
    {
        // add score logic
        ClearChildren();
        InMenu();
    }

    public void GameWon()
    {
        // score logic
        ClearChildren();
        StartGame();
    }
    public void CreateConnection()
    {
        Area2D player = GetNode<Area2D>("Player");
        Node2D wave = GetNode<Node2D>("Wave");
        player.Connect("PressShoot", this, nameof(OnPlayerShoot));
        player.Connect("HitGameOver", this, nameof(GameOver));
        wave.Connect("Defeat", this, nameof(GameWon));
        foreach (Node child in wave.GetChildren())
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
        uint randomEnemy = (uint)Math.Ceiling(GD.RandRange(0, GetNode<Node2D>("Wave").GetChildCount() - 1));
        if (enemy.GetIndex() == randomEnemy)
        {
            Rock rockInstance = (Rock)rock.Instance();
            AddChild(rockInstance);
            rockInstance.Position = location;
            rockInstance.Velocity.y = 1;
        }
    }
}
