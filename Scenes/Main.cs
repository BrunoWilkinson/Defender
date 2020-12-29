using Godot;
using System;

public class Main : Node
{
    public ulong score = 0;
    public ulong highScore;

    private PackedScene _waveScene = GD.Load<PackedScene>("res://Scenes/Wave/Wave.tscn");

    private PackedScene _playerScene = GD.Load<PackedScene>("res://Scenes/Player/Player.tscn");

    private Timer _waveTimer;

    private String _highScoreFilePath = "res://highscore.save";

    public override void _Ready()
    {
        GUI.UpdateScore(score);
        GetNode<CanvasLayer>("GUI").Connect("NewGame", this, nameof(StartGame));
        _waveTimer = GetNode<Timer>("WaveTimer");
        _waveTimer.Connect("timeout", this, nameof(UnPause));
        InMenu();
    }

    public void InMenu()
    {
        GUI.MenuGame();
    }

    public void StartGame()
    {
        LoadHighScore();
        _waveTimer.Start();
        GUI.InGame();
        GUI.UpdateHighScore(highScore);
        GUI.ShowGetReady(score);
        AddChild(_waveScene.Instance());
        AddChild(_playerScene.Instance());
        CreateConnection();
        GetNode<Node2D>("Wave").Hide();
        GetTree().Paused = true;
    }

    public void UnPause()
    {
        GetTree().Paused = false;
        GUI.HideGetReady();
        GetNode<Node2D>("Wave").Show();
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
        if (score > highScore)
        {
            SaveHighScore();
        }
        ClearChildren();
        InMenu();
    }

    public void GameWon()
    {
        score += 1;
        GUI.UpdateScore(score);
        ClearChildren();
        StartGame();
    }

    public void SaveHighScore()
    {
        File file = new File();
        file.Open(_highScoreFilePath, File.ModeFlags.Write);
        file.Store64(score);
        file.Close();
    }

    public void LoadHighScore()
    {
        File file = new File();
        if (file.FileExists(_highScoreFilePath))
        {
            file.Open(_highScoreFilePath, File.ModeFlags.Read);
            highScore = file.Get64();
            file.Close();
        }
        else
        {
            highScore = 0;
        }
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
