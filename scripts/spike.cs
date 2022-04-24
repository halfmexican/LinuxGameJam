using Godot;
using System;

public class spike : StaticBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private Area2D _area2D;
	private AudioStreamPlayer2D _audioStreamPlayer2D;
	private AnimatedSprite _animatedSprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_area2D = GetNode<Area2D>("Area2D");
		_area2D.Connect("body_entered", this, "CollisionCheck");
		_animatedSprite.Play("default");

	}

	private void CollisionCheck(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			_animatedSprite.Play("blood");
			_audioStreamPlayer2D.Stream = ResourceLoader.Load("res://assets/sounds/spikestab.ogg") as AudioStream;
			_audioStreamPlayer2D.Play();
		}
	}
	
	
	

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
