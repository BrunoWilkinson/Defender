using Godot;
using System;

public class Menu : Control
{
    private TextureButton _soundButton;

    private Sprite _speaker;

    private Sprite _speakerOff;

    private bool _mute = false;

    public override void _Ready()
    {
        _soundButton = GetNode<TextureButton>("SoundButton");
        _soundButton.Connect("pressed", this, nameof(changeSpeaker));
        _speaker = GetNode<Sprite>("Speaker");
        _speakerOff = GetNode<Sprite>("SpeakerOff");
    }

    public void changeSpeaker()
    {
        _mute = !_mute;
        AudioServer.SetBusMute(0, _mute);
        if (_mute)
        {
            _speaker.Visible = false;
            _speakerOff.Visible = true;
        }
        else
        {
            _speaker.Visible = true;
            _speakerOff.Visible = false;
        }
    }
}
