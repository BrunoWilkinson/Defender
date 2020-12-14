using Godot;
using System;

public class Player : Area2D
{
      [Signal]
      delegate void PressShoot(PackedScene bullet, Vector2 location);

      [Export]
      public float speed = 1000;

      [Export]
      public float spriteSize = 32;

      [Export]
      public float fireRate = 1;

      Vector2 screenSize;
      float currentTime;

      private PackedScene _missile = GD.Load<PackedScene>("res://Scenes/Missile/Missile.tscn");

      public override void _Ready()
    {
            screenSize = GetViewport().Size;
    }

      public override void _Process(float delta)
    {
            Controls(delta);
            Shoot(delta);
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
            if (Input.IsActionPressed("shoot") && currentTime > fireRate)
        {
                  currentTime = 0;
                  EmitSignal(nameof(PressShoot), _missile, Position);
        }
      }
}
