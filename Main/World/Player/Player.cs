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

    Vector2 screenSize;

    private PackedScene _missile = GD.Load<PackedScene>("res://Main/World/Player/Missile/Missile.tscn");

    private Area2D _shield;

    private bool _canShoot = true;

    public override void _Ready()
    {
        GetNode<Timer>("FireRate").Connect("timeout", this, nameof(CanShoot));
        screenSize = GetViewport().Size;
        _shield = GetNode<Area2D>("Shield");
        Connect("area_entered", this, nameof(Hit));
        _shield.Hide();
        _shield.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
    }

    public override void _Process(float delta)
    {
        Controls(delta);
        Shoot();
        Block();
    }

    public void CanShoot()
    {
        _canShoot = true;
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

    private void Shoot()
    {
        if (Input.IsActionJustReleased("shoot") && _canShoot && !Input.IsActionPressed("block"))
        {
# if DEBUG
            GD.Print("Log: Missile shoot");
#endif
            _canShoot = false;
            EmitSignal(nameof(OnShoot), _missile, Position);
        }
    }

    private void Block()
    {
        if (Input.IsActionPressed("block"))
        {
            _canShoot = false;
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
