using playercontrols;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls controls; 
    private Rigidbody2D rb; 
    private Vector2 direction; 

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCutOff = 0.5f; 
    [SerializeField] private float gravity = 3f; 
    [SerializeField] private float fallmultiplier = 2.5f; 

    private bool isFalling; 
    private bool cutJump; 
    private bool isGrounded; 
    private bool jumpPressed;    

   void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Getting the Rigidbody2D component attached to the player
        controls = new PlayerControls(); // Creating new instance of PlayerControls
        controls.Enable();
        rb.gravityScale = gravity; // Setting the gravity scale of the Rigidbody2D component to the value of the gravity variable
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = controls.Gameplay.Movement.ReadValue<Vector2>().x; // Reading the movement input from the controls
        if (controls.Gameplay.Jump.triggered && isGrounded) // Checking if the jump button is pressed and the player is grounded
        {
            jumpPressed = true; // Setting the jump input to true when the jump button is pressed
        }
        if (controls.Gameplay.Jump.WasReleasedThisFrame() && rb.linearVelocity.y > 0) // Checking if the jump button is released and the player is moving upwards
        {
            cutJump = true;
        
        }
         
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y); // Applying the movement to the Rigidbody2D component
        if (jumpPressed)
        {
            rb.gravityScale = gravity ; // Resetting the gravity scale to the default value when the player jumps
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Applying the jump force to the Rigidbody2D component
            jumpPressed = false; // Resetting the jump input
        }
        if (cutJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutOff); // Applying the jump cut-off to the Rigidbody2D component
            cutJump = false; // Resetting the jump cut-off input
        }
        if ( rb.linearVelocity.y < 0) // Checking if the player is falling
        {
            rb.gravityScale = gravity * fallmultiplier; // Applying the fall multiplier to the Rigidbody2D component
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Setting the isGrounded variable to true when the player collides with the ground
        }
    }

    void OnCollisionExit2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Setting the isGrounded variable to false when the player exits collision with the ground
        }
    }
}
