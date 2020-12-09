using Godot;
using System;

public class Enemy : Area2D
{
    [Signal]
    public delegate void Hit();

    [Signal]
    public delegate void CollideWall();

    public override void _Ready()
    {
        
    }
}
