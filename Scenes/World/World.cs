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
        _player.Connect("OnHit", this, nameof(GameOver));
        _wave.Connect("OnDefeat", this, nameof(WaveWon));
        foreach (Node child in _wave.GetChildren())
        {
            if (child.GetType().ToString() == "Enemy")
            {
                child.Connect("onShoot", this, nameof(OnEnemyShoot));
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

    public void GameOver()
    {
        EmitSignal(nameof(OnGameOver));
    }

    public void WaveWon()
    {
        EmitSignal(nameof(OnWaveWon));
    }

    public void OnPlayerShoot(PackedScene missile, Vector2 location)
    {
        Missile missileInstance = (Missile)missile.Instance();
        AddChild(missileInstance);
        missileInstance.Position = location;
        missileInstance.velocity.y = -1;
    }

    public void OnEnemyShoot(PackedScene rock, Vector2 location, Area2D enemy)
    {
        uint randomEnemy = (uint)Math.Ceiling(GD.RandRange(0, GetNode<Node2D>("Wave").GetChildCount() - 1));
        if (enemy.GetIndex() == randomEnemy)
        {
            Rock rockInstance = (Rock)rock.Instance();
            AddChild(rockInstance);
            rockInstance.Position = location;
            rockInstance.velocity.y = 1;
        }
    }
}
