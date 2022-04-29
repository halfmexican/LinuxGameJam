using Godot;
using System;

public class PlayerButton : Area2D
{
	private AnimatedSprite _mySprite;
	private bool _pressed = false;
	private bool _isPlayer = false;
	private bool _isCorpse = false;
	private bool _isPlayerCorpse = false;
	private bool _isBoulder = false;

	[Signal]
	public delegate void button_activated();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mySprite = GetNode<AnimatedSprite>("AnimatedSprite");
		Connect("area_entered", this, "ButtonCheck");
		Connect("area_exited", this, "ButtonExitCheck");
	}

	public override void _Process(float delta)
	{
		if (_pressed)
		{
			_mySprite.Play("pressed");
		}
		else
		{
			_mySprite.Play("notpressed");
		}
		
	}

	private void ButtonCheck(Node2D area)
	{
		if (area.IsInGroup("player") && area.IsInGroup("corpse"))
		{
			_isPlayerCorpse = true;
			_pressed = true;
			
			EmitSignal("button_activated");
			
		}
		else if (area.IsInGroup("player") && !_isCorpse && !_isPlayerCorpse)
		{
			_isPlayer = true;
			_pressed = true;
			EmitSignal("button_activated");
			
		}
		else if (area.IsInGroup("corpse") && !_isPlayer && !_isPlayerCorpse)
		{
			_isCorpse = true;
			_pressed = true;
			EmitSignal("button_activated");
			
		}
	}

	private void ButtonExitCheck(Node2D area)
	{
		if (_isPlayerCorpse && area.IsInGroup("player") && area.IsInGroup("corpse"))
		{
			_pressed = false;
			_isPlayerCorpse = false;
		}
		if (_isPlayer && area.IsInGroup("player"))
		{
			_pressed = false;
			_isPlayer = false;
		}
		else if (_isCorpse && area.IsInGroup("corpse"))
		{
			_pressed = false;
			_isCorpse = false;
		}
		
		
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
