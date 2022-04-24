using Godot;

namespace LinuxJam.scripts
{
	public class player2 : KinematicBody2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		
		private Vector2 _velocity = new Vector2(0f, 0f);
		[Export()]
		private float _gravity = 150.0f, _friction = 0.3f;
		[Export()]
		private float _maxSpeed = 35.0f;
		[Export()]
		private float _speed = 20.0f;
		[Export()]
		private float _jumpForce = 75.0f, _inertia = 100f, _jumpPadForce = 200f;
		private bool _isGrounded = false;
		private bool _isJumping = false;
		private int health = 1;
		private bool _dead = false;
		private RayCast2D _groundCheck, _armCheck, _headChecker;
		private AnimatedSprite _playerSprite;
		private AudioStreamPlayer2D _audioStreamPlayer2D;
		private Area2D _interactor;
	
	
	
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_groundCheck = GetNode<RayCast2D>("grounder");
			_armCheck = GetNode<RayCast2D>("arm");
			_headChecker = GetNode<RayCast2D>("headchecker");
			_playerSprite = GetNode<AnimatedSprite>("PlayerSprite");
			_audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("sounder");
			_interactor = GetNode<Area2D>("interactor");
			_interactor.Connect("area_entered", this, "InteractorCollision");
		}
		public override void _Process(float delta)
		{
			if (health <= 0 && !_dead)
			{
				var deadPlayer = ResourceLoader.Load<PackedScene>("res://assets/deadPlayer.tscn").Instance();
				var deadPlayerInstance = (Node2D)deadPlayer;
				GetParent().AddChild(deadPlayerInstance);
				deadPlayerInstance.Position = Position;
				_dead = true;
				var playerSpawner = ResourceLoader.Load<PackedScene>("res://assets/respawner.tscn").Instance();
				GetParent().AddChild(playerSpawner);
				
				QueueFree();
				
			}
		}

		public override void _PhysicsProcess(float delta)
		{
			GroundTest();
	 
			if(!_isGrounded)
				_velocity.y += delta * _gravity;

			if (Input.IsPhysicalKeyPressed(KeyList.D.GetHashCode()) && _velocity.x < _maxSpeed)
			{
				_velocity.x += _speed;
				_groundCheck.Position = new Vector2(-6, _groundCheck.Position.y);
				_armCheck.CastTo = new Vector2(18, 0);
				_playerSprite.FlipH = false;
				
				if (_isGrounded)
					_playerSprite.Play("walking", false);
				
			}
			else if (Input.IsPhysicalKeyPressed(KeyList.A.GetHashCode()) && -_velocity.x < _maxSpeed)
			{
				_velocity.x += _speed * -1;
				_groundCheck.Position = new Vector2(6, _groundCheck.Position.y);
				_armCheck.CastTo = new Vector2(-18, 0);
				_playerSprite.FlipH = true;
				if (_isGrounded)
					_playerSprite.Play("walking", false);
			
			}
			else if (_isGrounded)
			{
				_velocity.x = Mathf.Lerp(_velocity.x, 0, _friction);
				_isJumping = false;
				if (!Input.IsPhysicalKeyPressed(KeyList.A.GetHashCode()) && !Input.IsPhysicalKeyPressed(KeyList.D.GetHashCode()))
				{
					_playerSprite.Play("default");
				}
			}
		
			if (Input.IsPhysicalKeyPressed(KeyList.Space.GetHashCode()) && _isGrounded && !_isJumping)
			{
				_velocity.y = -_jumpForce;
				_isJumping = true;
				JumpSound();
				
			}

			if (IsOnCeiling())
			{
				_velocity.y *= -1;
			}

			if (_isJumping)
			{
				_playerSprite.Play("jump");
			}
		
			

			MoveAndSlide(_velocity, Vector2.Up, false, 4, 0.785f, false);
			
			MoveShit();

		}

		public void GroundTest()
		{
			_isGrounded = _groundCheck.IsColliding();
			if (_groundCheck.IsColliding())
			{
				_isGrounded = _groundCheck.IsColliding();
			}
			else if (IsOnFloor())
			{
				_isGrounded = IsOnFloor();
			}
			else
			{
				_isGrounded = false;
			}
		}

		private void MoveShit()
		{
			for (int i = 0; i < GetSlideCount(); i++)
			{
				var hit = GetSlideCollision(i);

				if (hit.Collider is RigidBody2D hitObject && hitObject.IsInGroup("pushable") && _armCheck.IsColliding())
				{
					hitObject.ApplyCentralImpulse(new Vector2(-hit.Normal.x * _inertia, 0));
				}
			   
				
			}
		}

		public void JumpSound()
		{
			RandomNumberGenerator random = new RandomNumberGenerator();
			random.Randomize();
			_audioStreamPlayer2D.Stream = ResourceLoader.Load("res://assets/sounds/jump.wav") as AudioStream;
			_audioStreamPlayer2D.PitchScale = random.RandfRange(0.8f, 1.2f);
			_audioStreamPlayer2D.Play();
		}

		private void InteractorCollision(Node2D area)
		{
			if (area.IsInGroup("jumpPad"))
			{
				_velocity.y = -_jumpPadForce ;
				_isJumping = true;
			}

			if (area.IsInGroup("enemy") || area.IsInGroup("obstacle"))
			{
				health--;
			}

			if (area.IsInGroup("instantkill"))
			{
				health = 0;
				_dead = true;
				_playerSprite.Visible = false;

			}

			if (area.IsInGroup("enemykiller"))
			{
				_velocity.y = -_jumpPadForce / 2;
				
				area.GetParent().QueueFree();
			}
		}
		
	}
}

