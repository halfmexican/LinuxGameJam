using Godot;
using System;
namespace LinuxJam.scripts
{
	public class Player2D : KinematicBody2D
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

		[Export()] private float _jumpForce = 75.0f, _inertia = 100f;
		private bool _isJumping = false;
		private int _health = 1, _coins = 0;
		private bool _dead = false;
		public bool CanMove = true;
		private RayCast2D  _rightArmCheck,_leftArmCheck, _grounder ;
		private AnimatedSprite _playerSprite;
		private AudioStreamPlayer2D _audioStreamPlayer2D, _musicPlayer2D;
		private Area2D _interactor;
		private Camera2D _camera2D;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

			RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
			randomNumberGenerator.Randomize();

			_health = PlayerVariables.HP;
			_camera2D = GetNode<Camera2D>("Camera2D");
			_grounder = GetNode<RayCast2D>("Grounder");
			_rightArmCheck = GetNode<RayCast2D>("RightArm");
			_leftArmCheck = GetNode<RayCast2D>("LeftArm");
			_playerSprite = GetNode<AnimatedSprite>("PlayerSprite");
			
			_audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("sounder");
			_musicPlayer2D = GetNode<AudioStreamPlayer2D>("MusicPlayer");
			_musicPlayer2D.Stream = ResourceLoader.Load("res://assets/sounds/Little_Bits_Main_Theme.ogg") as AudioStream;
			_musicPlayer2D.Seek(randomNumberGenerator.RandfRange(0, 50f));
			_musicPlayer2D.Play();
			_interactor = GetNode<Area2D>("interactor");
			_interactor.Connect("area_entered", this, "InteractorCollision");
		}
		public override void _Process(float delta)
		{
			if (_health <= 0 && !_dead)
			{
				KillPlayer(true);
			}

			if (!_grounder.IsColliding())
			{
				_playerSprite.Play("jump");
			}

			PlayerVariables.HP = _health;

		}

		public override void _PhysicsProcess(float delta)
		{
			_velocity.y += delta * _gravity;

			if (CanMove)
			{
				if (Input.IsPhysicalKeyPressed(KeyList.D.GetHashCode()) && _velocity.x < _maxSpeed && !_rightArmCheck.IsColliding())
				{
					_velocity.x += _speed;
					_playerSprite.FlipH = false;
					_grounder.Position = new Vector2(-7, 0);
				
					if (IsGrounded())
					{
						_isJumping = false;
						_playerSprite.Play("walking", false);
					}
				}
				else if (Input.IsPhysicalKeyPressed(KeyList.A.GetHashCode()) && -_velocity.x < _maxSpeed && !_leftArmCheck.IsColliding())
				{
					_velocity.x += _speed * -1;
					_playerSprite.FlipH = true;
					_grounder.Position = new Vector2(7, 0);

					if(IsGrounded())
					{
						_isJumping = false;
						_playerSprite.Play("walking", false);
					}
				}
				else if (IsGrounded())
				{
					_velocity.x = Mathf.Lerp(_velocity.x, 0, _friction);
					if (!Input.IsPhysicalKeyPressed(KeyList.A.GetHashCode()) && !Input.IsPhysicalKeyPressed(KeyList.D.GetHashCode()))
					{
						_playerSprite.Play("default");
						_isJumping = false;
					}
				}
			
				if (Input.IsActionJustPressed("player_jump") && IsGrounded() && !_isJumping)
				{
					_velocity.y = -_jumpForce;
					_isJumping = true;
					_playerSprite.Play("jump");
					JumpSound();
				}
			
				_velocity = MoveAndSlide(_velocity, Vector2.Up, false, 4, 0.785f, false);
			
				if (IsOnCeiling())
				{
					_velocity.y *= -1;
				}
			
				MoveShit();
			}
			
		}

		private void MoveShit()
		{
			for (int i = 0; i < GetSlideCount(); i++)
			{
				var hit = GetSlideCollision(i);
				if (hit.Collider is RigidBody2D hitObject)
				{
					if (hitObject.IsInGroup("pushable"))
					{
						hitObject.ApplyCentralImpulse(new Vector2(-hit.Normal.x * _inertia, 0));
					}

					if (hitObject.IsInGroup("corpse"))
					{
						hitObject.ApplyCentralImpulse(new Vector2(-hit.Normal.x * _inertia, 0));
					}

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

			public void SetVelocity(Vector2 vel)
			{
				_velocity = vel;
			}

			public void SetYVelocity(float yVel)
			{
				_velocity.y = yVel;
			}
		
			public void SetXVelocity(float xVel)
			{
				_velocity.x = xVel;
			}

			public void AddCoins(int num)
			{
				_coins += num;
			}

			private bool IsGrounded()
			{
				if (_grounder.IsColliding())
				{
					return _grounder.IsColliding();
				}
				else if(IsOnFloor())
				{
					return IsOnFloor();
				}
				else
				{
					return false;
				}
			}

			public void KillPlayer(bool leaveCorpse)
			{
				if (leaveCorpse)
				{
					var deadPlayer = ResourceLoader.Load<PackedScene>("res://assets/deadPlayer.tscn").Instance();
					var deadPlayerInstance = (Node2D)deadPlayer;
					GetParent().AddChild(deadPlayerInstance);
					deadPlayerInstance.Position = Position;
				
					RemoveChild(_camera2D);
					deadPlayerInstance.AddChild(_camera2D);
					_camera2D.Owner = deadPlayerInstance;
				}
				else
				{
					RemoveChild(_camera2D);
					_camera2D.GlobalPosition = GlobalPosition;
					GetParent().AddChild(_camera2D);
					_camera2D.Owner = GetParent();
				}
			
				_dead = true;
				var playerSpawner = ResourceLoader.Load<PackedScene>("res://assets/respawner.tscn").Instance();
				GetParent().AddChild(playerSpawner);
			
				QueueFree();
			}


			private void InteractorCollision(Node2D area)
			{
				if (area.IsInGroup("jumpPad"))
				{
					//_velocity.y = -_jumpPadForce ;
					_isJumping = true;
				}

				if (area.IsInGroup("enemy") || area.IsInGroup("obstacle"))
				{
					_health--;
				}

				if (area.IsInGroup("instantkill"))
				{
					_health = 0;
					_dead = true;
					_playerSprite.Visible = false;

				}

				if (area.IsInGroup("enemykiller") && GlobalPosition.y < area.GlobalPosition.y)
				{
					//SetYVelocity(-150f);
				
					//area.GetParent().QueueFree();
				}
			}
		
	}
}

