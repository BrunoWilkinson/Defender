using Godot;
using System;

public class Player : Area2D
{
    [Signal]
    delegate void OnShoot(PackedScene bullet, Vector2 location);

    [Signal]
    delegate void OnHit();
    [Signal]
    delegate void OnGameOver();

    [Export]
    public float speed = 500;

    [Export]
    public float spriteSize = 32;

    Vector2 screenSize;

    private PackedScene _missile = GD.Load<PackedScene>("res://Main/World/Player/Missile/Missile.tscn");

    private Area2D _shield;

    private AnimatedSprite _anim;

    private AudioStreamPlayer2D _shootAudio;

    private AudioStreamPlayer2D _shieldAudio;

    private AudioStreamPlayer2D _deathAudio;

    private bool _canShoot = true;

    private bool _isDead = false;

    public override void _Ready()
    {
        GetNode<Timer>("FireRate").Connect("timeout", this, nameof(CanShoot));
        screenSize = GetViewport().Size;
        _anim = GetNode<AnimatedSprite>("AnimatedSprite");
        _anim.Connect("animation_finished", this, nameof(OnAnimationFinished));
        _shootAudio = GetNode<AudioStreamPlayer2D>("ShootAudio");
        _shieldAudio = GetNode<AudioStreamPlayer2D>("ShieldAudio");
        _deathAudio = GetNode<AudioStreamPlayer2D>("DeathAudio");
        _shield = GetNode<Area2D>("Shield");
        Connect("area_entered", this, nameof(Hit));
        _shield.Hide();
        _shield.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
    }

    public override void _Process(float delta)
    {
        if (!_isDead)
        {
            Controls(delta);
            Shoot();
            Block();
        }
    }

    public void CanShoot()
    {
        _canShoot = true;
    }

    public void OnAnimationFinished()
    {
        if (_anim.Animation == "death")
        {
            QueueFree();
            EmitSignal(nameof(OnGameOver));
        }
    }

    public void Hit(Area2D area)
    {
        if (area is Enemy || area is Rock)
        {
            PauseMode = PauseModeEnum.Process;
            _isDead = true;
            area.QueueFree();
            _deathAudio.Play();
            _anim.Play("death");
            EmitSignal(nameof(OnHit));
        }
    }

    private void Controls(float delta)
    {
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += 1;
            _anim.Play("default");
        }
        if (Input.IsActionPressed("move_left"))
        {
            velocity.x -= 1;
            _anim.Play("default");
        }
        Position += velocity * speed * delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, spriteSize, screenSize.x - spriteSize),
            y: Position.y
        );

        if (Input.IsActionJustReleased("move_right") || Input.IsActionJustReleased("move_left"))
        {
            _anim.Stop();
        }
    }

    private void Shoot()
    {
        if (Input.IsActionJustReleased("shoot") && _canShoot && !Input.IsActionPressed("block"))
        {
            _shootAudio.Play();
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
            if (!_shieldAudio.Playing)
            {
                _shieldAudio.Playing = true;
            }
            _shield.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
        }
        else
        {
            _shield.Hide();
            _shieldAudio.Playing = false;
            _shield.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
        }
    }
}
