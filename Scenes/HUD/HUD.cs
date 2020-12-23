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

    public static void UpdateScore(ulong score)
    {
        _inGame.GetNode<Label>("Score").Text = $"Score: {score}";
    }

    public static void UpdateHighScore(ulong highScore)
    {
        _inGame.GetNode<Label>("HighScore").Text = $"HighScore: {highScore}";
    }

    public void OnNewGame()
    {
        EmitSignal(nameof(NewGame));
    }
}
