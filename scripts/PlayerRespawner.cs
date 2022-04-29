using Godot;

namespace LinuxJam.scripts
{
	public class PlayerRespawner : Node2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			
		}

		public override void _Process(float delta)
		{	
			if (Input.IsPhysicalKeyPressed(KeyList.R.GetHashCode()))
			{
				var spawnPoint = GetParent().GetNode("SpawnPoint") as Node2D;
				var playerClass = ResourceLoader.Load("res://assets/player.tscn") as PackedScene;
				var player = playerClass.Instance() as Node2D;
				player.GlobalPosition = spawnPoint.GlobalPosition;
				PlayerVariables.HP = 1;
				GetParent().AddChild(player);
				QueueFree();
			}
		}



//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  
	}
}
