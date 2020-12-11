using Godot;
using System;

public class Walls : Node2D
{
  [Signal]
  public delegate void rightWall(Area2D area);

  [Signal]
  public delegate void leftWall(Area2D area);

  public void OnRight(Area2D area)
  {
    if (area.Name == "Wave")
    {
      EmitSignal(nameof(rightWall), area);
    }

  }

  public void OnLeft(Area2D area)
  {
    if (area.Name == "Wave")
    {
      EmitSignal(nameof(leftWall), area);
    }

  }

  public override void _Ready()
  {
    GetNode<Area2D>("Right").Connect("area_entered", this, nameof(OnRight));
    GetNode<Area2D>("Left").Connect("area_entered", this, nameof(OnLeft));
  }

}