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
    [SerializeField] private float coyoteTime = 0.15f;

    private float coyoteTimeCounter;
    private bool isFalling; 
    private bool cutJump; 
    private bool isGrounded; 
    private bool jumpPressed;    

   void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Getting the Rigidbody2D component attached to the player
        controls = new PlayerControls(); // Creating new instance of PlayerControls
        controls.Enable();
        rb.gravityScale = gravity; 
    }

    // Update is called once per frame
    void Update()
    {
        
        direction.x = controls.Gameplay.Movement.ReadValue<Vector2>().x; 
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; 
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; 
        }
        if (controls.Gameplay.Jump.triggered && (isGrounded || coyoteTimeCounter > 0)) 
        {
            jumpPressed = true; 
            coyoteTimeCounter = 0; 
        }
        if (controls.Gameplay.Jump.WasReleasedThisFrame() && rb.linearVelocity.y > 0) 
        {
            cutJump = true;
        
        }
         
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y); // Applying the movement to the Rigidbody2D component
        if (jumpPressed)
        {
            rb.gravityScale = gravity ; 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Applying the jump force to the Rigidbody2D component
            jumpPressed = false; 
        }
        if (cutJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutOff); // Applying the jump cut-off to the Rigidbody2D component
            cutJump = false; 
        }
        if ( rb.linearVelocity.y < 0) // Checking if the player is falling
        {
            rb.gravityScale = gravity * fallmultiplier; 
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; 
        }
    }

    void OnCollisionExit2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; 
        }
    }
}
