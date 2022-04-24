using Godot;
using System;

public class Coin : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private AudioStreamPlayer2D _audioStreamPlayer2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_audioStreamPlayer2D.Stream = ResourceLoader.Load<AudioStream>("res://assets/sounds/pickupCoin.wav");
		Connect("body_entered", GetParent().GetNode("player"), "_on_Coin_body_entered");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void _on_Coin_body_entered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			QueueFree();
			
		}
		
	}

}



