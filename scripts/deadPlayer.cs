using Godot;
using System;

public class deadPlayer : RigidBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();
	private Area2D _area2D;
	private float _jumpForce = 200f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_randomNumberGenerator.Randomize();
		LinearVelocity = new Vector2(_randomNumberGenerator.RandiRange(-50, 50),
			_randomNumberGenerator.RandiRange(-150, -75));
		AngularVelocity = _randomNumberGenerator.RandfRange(-10, 10);
		_area2D = GetNode<Area2D>("Area2D");
		_area2D.Connect("area_entered", this, "InteractorCollision");
	}

	private void InteractorCollision(Node2D area)
	{
		if (area.IsInGroup("jumpPad") )
		{
			LinearVelocity = new Vector2(LinearVelocity.x, -_jumpForce);
		}

		if (area.IsInGroup("enemy"))
		{
			LinearVelocity = new Vector2(LinearVelocity.x, -_jumpForce/2);
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
