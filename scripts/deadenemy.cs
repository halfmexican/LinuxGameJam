using Godot;
using System;

public class deadenemy : RigidBody2D
{

	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();
	private Area2D _area2D;
	private AnimatedSprite _mySprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_randomNumberGenerator.Randomize();
		LinearVelocity = new Vector2(_randomNumberGenerator.RandiRange(-50, 50),
			_randomNumberGenerator.RandiRange(-150, -75));
		
		AngularVelocity = _randomNumberGenerator.RandfRange(-10, 10);

		_mySprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_area2D = GetNode<Area2D>("Area2D");
		//_area2D.Connect("area_entered", this, "InteractorCollision");
		
		_randomNumberGenerator.Randomize();
		if (_randomNumberGenerator.RandiRange(0, 100) < 49)
		{
			_mySprite.FlipH = true;
		}
		else
		{
			_mySprite.FlipH = false;
		}
		
		
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


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
