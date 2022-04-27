using Godot;

namespace LinuxJam.scripts
{
	public class boulder : RigidBody2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		private AnimatedSprite _mySprite;
		private Area2D _crush;
		private TileMap _tileMap;
		private bool _killedEnemy = false;
		private bool _isStatic = false;
		private Vector2 enemyPos;
		private int _bloodSpread;
		private RandomNumberGenerator _numberGenerator = new RandomNumberGenerator();
		private AudioStreamPlayer2D _audioStreamPlayer2D;
		private RayCast2D _rayCast2D;
		private bool _playSound = true;
		[Export()]
		private bool buttonLinked = false;
		[Export()] 
		private int buttonNumber = 1;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
			_mySprite = GetNode<AnimatedSprite>("BoulderSprite");
			_crush = GetNode<Area2D>("KidCrusher");
			_tileMap = GetParent().GetNode<TileMap>("TileMap");
			_mySprite.Play("default");
			_crush.Connect("area_entered", this, "CrushEnemy");
			_rayCast2D = GetNode<RayCast2D>("RayCast2D");
			this.Connect("body_entered", this, "Blood");

			
			if (buttonLinked)
			{
				_isStatic = true;
				var button = GetParent().GetNode("button" + buttonNumber.ToString());
				button.Connect("button_activated", this, "ButtonPressed");
			}
			
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			if (_isStatic)
			{
				this.Mode = ModeEnum.Static;
			}
			else
			{
				this.Mode = ModeEnum.Character;
			}

		}

		private void ButtonPressed()
		{
			_isStatic = false;
		}
		
		private void Blood(Node body)
		{
			if (body is TileMap && _killedEnemy)
			{
				_mySprite.Play("blood");

				if (_bloodSpread > 0)
				{
					var blood = (PackedScene)ResourceLoader.Load("res://assets/Blood.tscn") ;
					Sprite bloodInstance = (Sprite)blood.Instance();
				
					GetParent().AddChild(bloodInstance);
					bloodInstance.Position = new Vector2(Position.x, _rayCast2D.GetCollisionPoint().y+8);
					_bloodSpread--;
				}

				if (_playSound)
				{
					RandomNumberGenerator random = new RandomNumberGenerator();
					random.Randomize();
					_audioStreamPlayer2D.Stream = ResourceLoader.Load("res://assets/sounds/splat.ogg") as AudioStream;
					_audioStreamPlayer2D.PitchScale = random.RandfRange(0.8f, 1.2f);
					_audioStreamPlayer2D.Play();
					_playSound = false;
				}
				
			}

		}

		private void CrushEnemy(Node body)
		{
			_numberGenerator.Randomize();
			_bloodSpread = _numberGenerator.RandiRange(1, 4);
			
			if (body.IsInGroup("enemy") || body.IsInGroup("enemykiller"))
			{
				_killedEnemy = true;
				KinematicBody2D enemy = body.GetParent() as KinematicBody2D;
				enemyPos = enemy.Position;
				enemy.QueueFree();
				_playSound = true;
			}
			else if (body.IsInGroup("player"))
			{
				_killedEnemy = true;
				if (body.GetParent() is Player2D enemy)
				{
					enemyPos = enemy.Position; // used enenmy variable name for player
					enemy.KillPlayer(false);
				}

				var playerSpawner = ResourceLoader.Load<PackedScene>("res://assets/respawner.tscn").Instance();
				GetParent().AddChild(playerSpawner);
				_playSound = true;

			}
		}
	}
}





