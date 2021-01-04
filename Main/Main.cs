using Godot;
using System;

public class Main : Node
{
    public ulong score = 0;
    public ulong highScore;

    private Timer _waveTimer;

    private Node2D _world;

    private CanvasLayer _gui;

    private String _highScoreFilePath = "res://highscore.save";

    public override void _Ready()
    {
        _gui = GetNode<CanvasLayer>("GUI");
        _world = GetNode<Node2D>("World");
        _waveTimer = GetNode<Timer>("WaveTimer");
        _waveTimer.Connect("timeout", this, nameof(UnPause));
        CreateConnection();
        GUI.UpdateScore(score);
        GUI.MenuGame();
    }

    public void CreateConnection()
    {
        _gui.Connect("NewGame", this, nameof(StartGame));
        _world.Connect("OnGameOver", this, nameof(GameOver));
        _world.Connect("OnWaveWon", this, nameof(WaveWon));
    }

    public void StartGame()
    {
        LoadHighScore();
        _waveTimer.Start();
        GUI.InGame();
        GUI.UpdateHighScore(highScore);
        GUI.ShowGetReady(score);
        _world.Hide();
        GetTree().Paused = true;
    }

    public void UnPause()
    {
        GetTree().Paused = false;
        GUI.HideGetReady();
        _world.Show();
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
        GUI.MenuGame();
    }

    public void WaveWon()
    {
        score += 1;
        GUI.UpdateScore(score);
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
}