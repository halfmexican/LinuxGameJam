using Godot;
using System;
using LinuxJam.scripts;

public class HUD : Label
{
	private PlayerVariables playerVar;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerVar = (PlayerVariables)GetNode("/root/PlayerVariables");
	}
	
	public override void _Process(float delta)
	{
		Text = "HP:" + PlayerVariables.HP.ToString() + "\nCoins:" + PlayerVariables.coins.ToString();
	}
	
	
}
