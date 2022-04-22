using Godot;

namespace LinuxJam
{
    public class player2 : KinematicBody2D
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        
        private Vector2 velocity = new Vector2(0f, 0f);
        [Export()]
        private float gravity = 100.0f, friction = 0.3f;
        [Export()]
        private float maxSpeed = 35.0f;
        [Export()]
        private float speed = 20.0f;

        [Export()] private float jumpForce = 50.0f, inertia = 100f;
        private bool isGrounded = false;
        private bool isJumping = false;
        private RayCast2D groundCheck, armCheck;
        private AnimatedSprite playerSprite;
    
    
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            groundCheck = GetNode<RayCast2D>("grounder");
            armCheck = GetNode<RayCast2D>("arm");
            playerSprite = GetNode<AnimatedSprite>("PlayerSprite");
        }

        public override void _PhysicsProcess(float delta)
        {
            GroundTest();
     
            if(!isGrounded)
                velocity.y += delta * gravity;

            if (Input.IsPhysicalKeyPressed(KeyList.D.GetHashCode()) && velocity.x < maxSpeed)
            {
                velocity.x += speed;
                groundCheck.Position = new Vector2(-6, groundCheck.Position.y);
                armCheck.CastTo = new Vector2(18, 0);
                playerSprite.FlipH = false;
                
                if (isGrounded)
                    playerSprite.Play("walking", false);
                
            }
            else if (Input.IsPhysicalKeyPressed(KeyList.A.GetHashCode()) && -velocity.x < maxSpeed)
            {
                velocity.x += speed * -1;
                groundCheck.Position = new Vector2(6, groundCheck.Position.y);
                armCheck.CastTo = new Vector2(-18, 0);
                playerSprite.FlipH = true;
                if (isGrounded)
                    playerSprite.Play("walking", false);
            
            }
            else if (isGrounded)
            {
                velocity.x = Mathf.Lerp(velocity.x, 0, friction);
                isJumping = false;
                if (!Input.IsPhysicalKeyPressed(KeyList.A.GetHashCode()) && !Input.IsPhysicalKeyPressed(KeyList.D.GetHashCode()))
                {
                    playerSprite.Play("default");
                }
            }
        
            if (Input.IsPhysicalKeyPressed(KeyList.Space.GetHashCode()) && isGrounded && !isJumping)
            {
                velocity.y = -jumpForce;
                isJumping = true;
                
            }

            if (isJumping)
            {
                playerSprite.Play("jump");
            }
        
            

            MoveAndSlide(velocity, Vector2.Up, false, 4, 0.785f, false);
            
            MoveShit();

        }

        public void GroundTest()
        {
            isGrounded = groundCheck.IsColliding();
        
        }

        public void MoveShit()
        {
            for (int i = 0; i < GetSlideCount(); i++)
            {
                var hit = GetSlideCollision(i);

                if (hit.Collider is RigidBody2D hitObject && hitObject.IsInGroup("pushable") && armCheck.IsColliding())
                {
                    hitObject.ApplyCentralImpulse(new Vector2(velocity.x, 0));
                }
               
            }
        }

        

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    
    }
}
