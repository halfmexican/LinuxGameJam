using Godot;

namespace LinuxJam.scripts
{
    public class PlayerVariables : Node
    {
        public Node CurrentScene { get; set; }
        public static int coins = 0;
        public static int HP = 3;

        public override void _Ready()
        {
           

        }
    
    public void AddCoins(int n)
        {
            coins += n;
        }
        
        public void RemoveCoins(int n)
        {
            coins -= n;
        }

        public void AddHP(int n)
        {
            HP += n;
        }
        
        public void SubHP(int n)
        {
            HP -= n;
        }
        
    }
    
    
}