using Godot;

namespace LinuxJam.scripts
{
	public class LevelSwitcher : Node
	{
		private Node curLevel;
		private int levelNum = 0;
		public override void _Ready()
		{
			Viewport root = GetTree().Root;
			var curLevelPath = ResourceLoader.Load("res://Levels/level" + levelNum +".tscn") as PackedScene;
			curLevel = root.GetChild(root.GetChildCount() - 1);

		}

		public override void _Process(float delta)
		{
			
			if (Input.IsPhysicalKeyPressed(KeyList.L.GetHashCode()))
			{
				GotoScene(levelNum);
			}
		}

		public void GetCurLevel()
		{
			Viewport root = GetTree().Root;
			var curLevelPath = ResourceLoader.Load("res://Levels/level" + levelNum +".tscn") as PackedScene;
			curLevel = root.GetChild(root.GetChildCount() - 1);
		}
		
		
		public void GotoScene(int nextlevel)
		{
			// This function will usually be called from a signal callback,
			// or some other function from the current scene.
			// Deleting the current scene at this point is
			// a bad idea, because it may still be executing code.
			// This will result in a crash or unexpected behavior.

			// The solution is to defer the load to a later time, when
			// we can be sure that no code from the current scene is running:

			CallDeferred(nameof(DeferredGotoScene),nextlevel);
		}

		public void DeferredGotoScene(int level)
		{
			this.levelNum = level;
			// It is now safe to remove the current scene
			curLevel.Free();

			// Load a new scene.
			var nextScene = (PackedScene)ResourceLoader.Load("res://Levels/level" + level +".tscn");

			// Instance the new scene.
			curLevel = nextScene.Instance();

			// Add it to the active scene, as child of root.
			GetTree().Root.AddChild(curLevel);

			// Optionally, to make it compatible with the SceneTree.change_scene() API.
			GetTree().CurrentScene = curLevel;
		}

		public void ReloadLevel()
		{
			
			if (Input.IsPhysicalKeyPressed(KeyList.L.GetHashCode()))
			{
				GetTree().ReloadCurrentScene();
				
			}
		}
		
		
	}
}
