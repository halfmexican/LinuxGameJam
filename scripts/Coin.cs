using Godot;
using System;
using LinuxJam.scripts;

public class Coin : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private bool _collected;
	private AudioStreamPlayer2D _audioStreamPlayer2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_audioStreamPlayer2D.Stream = ResourceLoader.Load<AudioStream>("res://assets/sounds/pickupCoin.wav");
		Connect("area_entered", this, "CollectCheck");
		//_audioStreamPlayer2D.Connect("finished", this, " sound");

	}
	
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void CollectCheck(Node body)
	{
		if (body.IsInGroup("player") && !_collected)
		{
			if (body.GetParent() is Player2D)
			{
				var player = body.GetParent() as Player2D;
				//player.AddCoins(1);

				PlayerVariables.coins += 1;
			}

			RandomNumberGenerator random = new RandomNumberGenerator();
			random.Randomize();
			_audioStreamPlayer2D.PitchScale = random.RandfRange(0.8f, 1.2f);
			_audioStreamPlayer2D.Play();
			_collected = true;
			Visible = false;

		}
		
	}

}



