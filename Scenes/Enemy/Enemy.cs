using Godot;
using System;

public class Enemy : Area2D
{
    public void onHit(Area2D area) 
    {
        if (area.Name == "Missile")
        {
            QueueFree();
        }
        
    }
}
