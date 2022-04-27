using Godot;

namespace LinuxJam.scripts
{
	public class boulder : RigidBody2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		private AnimatedSprite _mySprite;
		private Area2D crush;
		private TileMap _tileMap;
		private bool killedEnemy = false;
		private bool isStatic = false;
		private Vector2 enemyPos;
		private int bloodSpread;
		private RandomNumberGenerator _numberGenerator = new RandomNumberGenerator();
		private AudioStreamPlayer2D _audioStreamPlayer2D;
		private RayCast2D _rayCast2D;
		private bool playSound = true;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
			_mySprite = GetNode<AnimatedSprite>("BoulderSprite");
			crush = GetNode<Area2D>("KidCrusher");
			_tileMap = GetParent().GetNode<TileMap>("TileMap");
			_mySprite.Play("default");
			crush.Connect("area_entered", this, "CrushEnemy");
			_rayCast2D = GetNode<RayCast2D>("RayCast2D");
			this.Connect("body_entered", this, "Blood");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			

		}

		private void Blood(Node body)
		{
			if (body is TileMap && killedEnemy)
			{
				_mySprite.Play("blood");

				if (bloodSpread > 0)
				{
					var blood = (PackedScene)ResourceLoader.Load("res://assets/Blood.tscn") ;
					Sprite bloodInstance = (Sprite)blood.Instance();
				
					GetParent().AddChild(bloodInstance);
					bloodInstance.Position = new Vector2(Position.x, _rayCast2D.GetCollisionPoint().y+8);
					bloodSpread--;
				}

				if (playSound)
				{
					RandomNumberGenerator random = new RandomNumberGenerator();
					random.Randomize();
					_audioStreamPlayer2D.Stream = ResourceLoader.Load("res://assets/sounds/splat.ogg") as AudioStream;
					_audioStreamPlayer2D.PitchScale = random.RandfRange(0.8f, 1.2f);
					_audioStreamPlayer2D.Play();
					playSound = false;
				}
				
			}

		}

		private void CrushEnemy(Node body)
		{
			_numberGenerator.Randomize();
			bloodSpread = _numberGenerator.RandiRange(1, 4);
			
			if (body.IsInGroup("enemy"))
			{
				killedEnemy = true;
				KinematicBody2D enemy = body.GetParent() as KinematicBody2D;
				enemyPos = enemy.Position;
				enemy.QueueFree();
				playSound = true;
			}
			else if (body.IsInGroup("player"))
			{
				killedEnemy = true;
				if (body.GetParent() is Player2D enemy)
				{
					enemyPos = enemy.Position; // used enenmy variable name for player
					enemy.KillPlayer(false);
				}

				var playerSpawner = ResourceLoader.Load<PackedScene>("res://assets/respawner.tscn").Instance();
				GetParent().AddChild(playerSpawner);
				playSound = true;

			}
		}
	}
}





