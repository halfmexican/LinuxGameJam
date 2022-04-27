using Godot;
using System;

public class testenemy : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Vector2 velocity = new Vector2(0f, 0f);
	private float gravity = 100.0f, friction = 0.3f;
	private Area2D crushcheck;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		crushcheck = GetNode<Area2D>("Area2D");
		//crushcheck.Connect("area_entered", this, "CollisionCheck");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }


		public override void _PhysicsProcess(float delta)
		{
			if (!IsOnFloor())
			{
				velocity.y += delta * gravity;

			}
			
			velocity = MoveAndSlide(velocity);
		}
		
		
}
