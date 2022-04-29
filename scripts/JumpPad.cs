using Godot;
using System;
using LinuxJam.scripts;

public class JumpPad : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private AnimatedSprite _myspSprite;
	private AudioStreamPlayer2D _audioStreamPlayer2D;
	[Export]
	float _jumpPadForce = -200f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_myspSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		Connect("area_entered", this, "CollisionCheck");
		_myspSprite.Connect("animation_finished", this, "AnimationFinished");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void CollisionCheck(Node2D area)
	{
		
		if (area.IsInGroup("player") || area.IsInGroup("corpse"))
		{
			if (area.GetParent() is Player2D)
			{
				var player = area.GetParent() as Player2D;
				player?.SetYVelocity(_jumpPadForce);
			}
			else if (area.GetParent() is deadPlayer)
			{
				var player = area.GetParent() as deadPlayer;
				player?.setYVelocity(_jumpPadForce);
			}
			else if (area.GetParent() is deadenemy)
			{
				var corpse = area.GetParent() as deadenemy;
				corpse?.setYVelocity(_jumpPadForce);
			}
			
			
			
			RandomNumberGenerator random = new RandomNumberGenerator();
			random.Randomize();
			_myspSprite.Play("bounce");
			_audioStreamPlayer2D.Stream = ResourceLoader.Load("res://assets/sounds/jumppad.wav") as AudioStream;
			_audioStreamPlayer2D.PitchScale = random.RandfRange(0.8f, 1.2f);
			_audioStreamPlayer2D.Play();
		}
		
	}

	private void AnimationFinished()
	{
		_myspSprite.Play("default");
	}
	
}
