using Godot;
using System;

public class World : Node2D
{
    [Signal]
    delegate void OnGameOver();

    [Signal]
    delegate void OnWaveWon();

    private Area2D _player;

    private Node2D _wave;

    public override void _Ready()
    {
        _player = GetNode<Area2D>("Player");
        _wave = GetNode<Node2D>("Wave");
        CreateConnection();
    }

    public void CreateConnection()
    {
        _player.Connect("OnShoot", this, nameof(OnPlayerShoot));
        _player.Connect("OnHit", this, nameof(PlayerHit));
        _player.Connect("OnGameOver", this, nameof(GameOver));
        Node2D walls = GetNode<Node2D>("Walls");
        walls.Connect("OnRightCollision", _wave, "OnCollideRight");
        walls.Connect("OnLeftCollision", _wave, "OnCollideLeft");
        _wave.Connect("OnDefeat", this, nameof(WaveWon));
        foreach (Node child in _wave.GetChildren())
        {
            if (child is Enemy)
            {
                child.Connect("OnShoot", this, nameof(OnEnemyShoot));
            }
        }
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

    public void PlayerHit()
    {
        GetTree().Paused = true;
    }

    public void GameOver()
    {
        GetTree().Paused = false;
        EmitSignal(nameof(OnGameOver));
    }

    public void WaveWon()
    {
        GetTree().Paused = true;
        EmitSignal(nameof(OnWaveWon));
    }

    public void OnPlayerShoot(PackedScene missile, Vector2 location)
    {
        Missile missileInstance = (Missile)missile.Instance();
        AddChild(missileInstance);
        missileInstance.Position = location;
    }

    public int GenerateRandomEnemy ()
    {
        Random rand = new Random();
        int enemyCount = GetNode<Node2D>("Wave").GetChildCount() - 1;
        return rand.Next(0, enemyCount);
    }

    public void OnEnemyShoot(PackedScene rock, Vector2 location, Area2D enemy)
    {
        GenerateRandomEnemy();
        if (enemy.GetIndex() == GenerateRandomEnemy() || enemy.GetIndex() == GenerateRandomEnemy())
        {
            Rock rockInstance = (Rock)rock.Instance();
            AddChild(rockInstance);
            rockInstance.Position = location;
        }
    }
}
