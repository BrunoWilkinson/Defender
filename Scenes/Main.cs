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

    private Node2D _wave;

    private Area2D _player;

    private Timer _waveTimer;

    public override void _Ready()
    {
        GetNode<CanvasLayer>("HUD").Connect("NewGame", this, nameof(StartGame));
        _waveTimer = GetNode<Timer>("WaveTimer");
        _waveTimer.Connect("timeout", this, nameof(UnPause));
        _wave = (Wave)_waveScene.Instance();
        _player = (Player)_playerScene.Instance();
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
        AddChild(_wave);
        AddChild(_player);
        CreateConnection();
        GetTree().Paused = true;
    }

    public void UnPause()
    {
        GD.Print("UnPAUSE");
        GetTree().Paused = false;
    }

    public void CreateConnection()
    {
        GetNode<Area2D>("Player").Connect("PressShoot", this, nameof(OnPlayerShoot));
        foreach (Node child in GetNode<Node2D>("Wave").GetChildren())
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
}
