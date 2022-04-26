using Godot;
using System;

public class deadPlayer : RigidBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();
	private Area2D _area2D;

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
	
	public void setVelocity(Vector2 vel)
	{
		LinearVelocity = vel;
	}

	public void setYVelocity(float yVel)
	{
		LinearVelocity = new Vector2(LinearVelocity.x ,yVel);
	}
		
	public void setXVelocity(float xVel)
	{
		LinearVelocity = new Vector2(xVel, LinearVelocity.y);
	}

	private void InteractorCollision(Node2D area)
	{
		
		if (area.IsInGroup("enemy"))
		{
			setYVelocity(-100); 
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
