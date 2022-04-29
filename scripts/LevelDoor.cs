using Godot;
using System;
using LinuxJam.scripts;

public class LevelDoor : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export()]
	private int _nextLevel = 1;

	private AnimatedSprite _doorSprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_doorSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		Connect("area_entered", this, "AreaCheck");
		_doorSprite.Connect("animation_finished", this, "LevelChange");
	}

	
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	public void AreaCheck(Node2D area)
	{
		if (area.IsInGroup("player"))
		{
			if (area.GetParent() is Player2D player)
			{
				player.Visible = false;
				player.CanMove = false;
			}
				
			_doorSprite.Play("nextlevel");
			
		}
		
	}

	public void LevelChange()
	{
		var levelSwitch = (LevelSwitcher)GetNode("/root/LevelSwitcher");
		levelSwitch.GotoScene(_nextLevel);
	}



}
