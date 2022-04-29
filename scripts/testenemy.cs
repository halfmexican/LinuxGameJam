using Godot;
using System;
using LinuxJam.scripts;

public class testenemy : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Vector2 velocity = new Vector2(0f, 0f);
	private float gravity = 100.0f, friction = 0.3f, _maxSpeed = 35.0f, _speed = 20.0f ;
	private Area2D _interactor;
	public AudioStreamPlayer2D _enemyAudio;
	private AnimatedSprite _animatedSprite;
	private RayCast2D _rayCast2D;
	private Area2D _enemyKiller;
	private int enemyState = 1;
	[Export] private bool isStatic = false;
	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{
		_interactor = GetNode<Area2D>("interactor");
		_interactor.Connect("area_entered", this, "InteractorCheck");
		_enemyKiller = GetNode<Area2D>("EnemyKiller");
		_enemyKiller.Connect("area_entered", this, "KillCheck");
		_enemyAudio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_rayCast2D = GetNode<RayCast2D>("grounder");

		if (isStatic)
		{
			enemyState = 3;
		}
		
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		velocity.y += delta * gravity;

		switch (enemyState)
		{
			case 1:
			{
				if (!_rayCast2D.IsColliding())
				{
					enemyState = 2;
				}

				break;
			}
			case 2:
			{
				if (!_rayCast2D.IsColliding())
				{
					enemyState = 1;
				}

				break;
			}
			case 3:
				enemyState = 3;
				break;
		}
		
		switch (enemyState)
		{
			case 1:
				MoveRight();
				break;
			case 2:
				MoveLeft();
				break;
			case 3:
				StandStill();
				break;
		}
   
	}
	
	public override void _PhysicsProcess(float delta)
	{

		velocity = MoveAndSlide(velocity);
	}

	public void MoveRight()
	{
		if (velocity.x < _maxSpeed)
		{
			velocity.x += _speed;
			_animatedSprite.FlipH = false;
			_animatedSprite.Play("walking");
			_rayCast2D.Position = new Vector2(9, 7);
			
		}
		
	}
	
	public void MoveLeft()
	{
		if (-velocity.x < _maxSpeed)
		{
			velocity.x -= _speed;
			_animatedSprite.FlipH = true;
			_animatedSprite.Play("walking");
			_rayCast2D.Position = new Vector2(-9, 7);
		}
		
	}

	public void StandStill()
	{
		velocity.x = 0;
		_animatedSprite.Play("default");
	}
	

	private void KillCheck(Node area)
	{
		if (area.IsInGroup("player") && area.GetParent() is Player2D)
		{
			var player = area.GetParent() as Player2D;
			player?.SetYVelocity(-150f);
			KillMe();

		}
	}
	
	private void KillMe()
	{
		var deadEnemy = ResourceLoader.Load<PackedScene>("res://assets/deadenemy.tscn").Instance();
		var deadEnemyInstance = deadEnemy as Node2D;
		
		GetParent().AddChild(deadEnemyInstance);
		if (deadEnemyInstance != null) 
			deadEnemyInstance.Position = Position;
		QueueFree();
	}

	private void InteractorCheck(Node2D area)
	{
		if (area.IsInGroup("obstacle"))
		{
			KillMe();
		}
	}

}
