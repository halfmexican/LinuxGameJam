using Godot;
using System;
using LinuxJam.scripts;

public class JumpPad : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private AnimatedSprite myspSprite;
	private AudioStreamPlayer2D _audioStreamPlayer2D;
	float _jumpPadForce = -200f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		myspSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		Connect("area_entered", this, "CollisionCheck");
		myspSprite.Connect("animation_finished", this, "AnimationFinished");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void CollisionCheck(Node2D body)
	{
		
		if (body.IsInGroup("player"))
		{
			if (body.GetParent() is Player2D)
			{
				var player = body.GetParent() as Player2D;
				player.setYVelocity(_jumpPadForce);
			}
			else if (body.GetParent() is deadPlayer)
			{
				var player = body.GetParent() as deadPlayer;
				player.setYVelocity(_jumpPadForce);
			}
			
			
			
			RandomNumberGenerator random = new RandomNumberGenerator();
			random.Randomize();
			myspSprite.Play("bounce");
			_audioStreamPlayer2D.Stream = ResourceLoader.Load("res://assets/sounds/jumppad.wav") as AudioStream;
			_audioStreamPlayer2D.PitchScale = random.RandfRange(0.8f, 1.2f);
			_audioStreamPlayer2D.Play();
		}
	}

	private void AnimationFinished()
	{
		myspSprite.Play("default");
	}
	
}
