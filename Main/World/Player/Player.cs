using Godot;
using System;

public class Player : Area2D
{
    [Signal]
    delegate void OnShoot(PackedScene bullet, Vector2 location);

    [Signal]
    delegate void OnHit();

    [Export]
    public float speed = 500;

    [Export]
    public float spriteSize = 32;

    [Export]
    public float fireRate = 1;

    Vector2 screenSize;
    float currentTime;

    private PackedScene _missile = GD.Load<PackedScene>("res://Scenes/Missile/Missile.tscn");

    private Area2D _shield;

    public override void _Ready()
    {
        currentTime = fireRate;
        screenSize = GetViewport().Size;
        _shield = GetNode<Area2D>("Shield");
        Connect("area_entered", this, nameof(Hit));
        _shield.Hide();
        _shield.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
    }

    public override void _Process(float delta)
    {
        Controls(delta);
        Shoot(delta);
        Block(delta);
    }

    public void Hit(Area2D area)
    {
        String type = area.GetType().ToString();
        if (type == "Enemy" || type == "Rock")
        {
            QueueFree();
            area.QueueFree();
            EmitSignal(nameof(OnHit));
        }
    }

    private void Controls(float delta)
    {
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += 1;
        }
        if (Input.IsActionPressed("move_left"))
        {
            velocity.x -= 1;
        }
        Position += velocity * speed * delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, spriteSize, screenSize.x - spriteSize),
            y: Position.y
        );
    }

    private void Shoot(float delta)
    {
        currentTime += delta;
        if (Input.IsActionJustReleased("shoot") && currentTime >= fireRate && !Input.IsActionPressed("block"))
        {
            currentTime = 0;
            EmitSignal(nameof(OnShoot), _missile, Position);
        }
    }

    private void Block(float detla)
    {
        if (Input.IsActionPressed("block"))
        {
            currentTime = 0;
            _shield.Show();
            _shield.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
        }
        else
        {
            _shield.Hide();
            _shield.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
        }
    }
}