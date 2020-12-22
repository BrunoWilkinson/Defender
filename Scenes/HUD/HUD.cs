using Godot;
using System;

public class HUD : CanvasLayer
{
    [Signal]
    delegate void NewGame();
    private static Control _menu;
    private static Control _inGame;
    public override void _Ready()
    {
        _menu = GetNode<Control>("Menu");
        _inGame = GetNode<Control>("InGame");
        _menu.GetNode<Button>("NewGame").Connect("pressed", this, nameof(OnNewGame));
    }

    public static void MenuGame()
    {
        _menu.Show();
        _inGame.Hide();
    }
    public static void InGame()
    {
        _menu.Hide();
        _inGame.Show();
    }

    public void OnNewGame()
    {
        EmitSignal(nameof(NewGame));
    }
}
