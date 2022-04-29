using Godot;
using System;
using LinuxJam.scripts;

public class MainMenuButton : Button
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("pressed", this, "MainMenuPressed");
	}

	public void MainMenuPressed()
	{
		var levelSwitch = (LevelSwitcher)GetNode("/root/LevelSwitcher");
		levelSwitch.GotoScene(1);
	}
	

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
