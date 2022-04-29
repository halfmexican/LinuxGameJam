using Godot;
using System;

public class Door : StaticBody2D
{
    [Export()]
    private int _buttonNumber;
    private AnimatedSprite _mySprite;

    private CollisionShape2D _shape2D;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _mySprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _shape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        var button = GetParent().GetNode("button" + _buttonNumber.ToString());
        button.Connect("button_activated", this, "ButtonPressed");
        button.Connect("button_deactivated", this, "ButtonNotPressed");
    }

    public void ButtonPressed()
    {
        _mySprite.Visible = false;
        //_shape2D.Disabled = true;
        _shape2D.SetDeferred("disabled", true);
    }

    public void ButtonNotPressed()
    {
        _mySprite.Visible = true;
       // _shape2D.Disabled = false;
        _shape2D.SetDeferred("disabled", false);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
