using Godot;
using System;

public class HUD : CanvasLayer
{
    private static Control _menu;
    private static Control _inGame;
    public override void _Ready()
    {
        _menu = GetNode<Control>("Menu");
        _inGame = GetNode<Control>("InGame");
        // StartGame();
        InGame();
    }

    public static void StartGame()
    {
        _menu.Show();
        _inGame.Hide();
    }
    public static void InGame()
    {
        _menu.Hide();
        _inGame.Show();
    }
}
