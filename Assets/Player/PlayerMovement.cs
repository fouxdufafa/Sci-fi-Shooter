using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float jumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField] private LayerMask whatIsGround;                  // A mask determining what is ground to the character
    
    public enum State
    {
        Grounded = 1,
        Jumping = 2,
        WallHugging = 3
    }

    private State currentState;

    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool isGrounded;            // Whether or not the player is grounded.
    private Animator animator;            // Reference to the player's animator component.
    private Rigidbody2D rigidbody2d;
    private bool isFacingRight = true;  // For determining which way the player is currently facing.

    private bool isHuggingWall = false;
    private GameObject isNearbyWall = null;
    [SerializeField] float wallHugForceMultiplier = 3f;

    private bool isAiming = false;
    [SerializeField] GameObject crosshair;
    [SerializeField] float crosshairRadius;

    // grounded -> jumping
    // jumping -> grounded, wall hugging
    // wall hugging -> jumping

    private void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("GroundCheck");
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                isGrounded = true;
        }
        animator.SetBool("Ground", isGrounded);

        // Set the vertical animation
        animator.SetFloat("vSpeed", rigidbody2d.velocity.y);
    }

    public void Move(float horizontal, float vertical, bool jump, TriggerState leftTrigger, float horizontalAim, float verticalAim)
    {
        // Prevent rigidbody from sleeping in a stationary wall-hug state
        if (leftTrigger == TriggerState.End && rigidbody2d.velocity.magnitude == 0)
        {
            rigidbody2d.WakeUp();
        }

        // Determine if player is hugging a nearby wall
        if (!isGrounded && isNearbyWall && (leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay))
        {
            isHuggingWall = true;
        }
        else
        {
            isHuggingWall = false;
        }

        // Determine if player is aiming (left trigger)
        // Can aim if either
        // 1. grounded (in which case, disable movement)
        // 2. wall hugging
        if ((leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay) && (isGrounded || isHuggingWall))
        {
            isAiming = true;

            if (isGrounded)
            {
                // Prevent sliding while aiming
                rigidbody2d.velocity = Vector2.zero;
                animator.SetFloat("Speed", 0f);
            }
            
            Vector3 offset = new Vector2(horizontalAim, verticalAim).normalized * crosshairRadius;
            crosshair.GetComponent<SpriteRenderer>().enabled = offset.magnitude > 0;
            crosshair.transform.position = transform.position + offset;
            if (horizontal > 0 && !isFacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (horizontal < 0 && isFacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        else
        {
            isAiming = false;
            crosshair.GetComponent<SpriteRenderer>().enabled = false;
        }

        //only move the player if not aiming or wallhugging
        if (!isAiming && !isHuggingWall)
        {
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            animator.SetFloat("Speed", Mathf.Abs(horizontal));

            // Move the character
            rigidbody2d.velocity = new Vector2(horizontal * maxSpeed, rigidbody2d.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (horizontal > 0 && !isFacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (horizontal < 0 && isFacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        // If the player should jump...
        if (isGrounded && !isAiming && jump && animator.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            isGrounded = false;
            animator.SetBool("Ground", false);
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
            rigidbody2d.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WallJumpable")
        {
            print("Hit jumpable wall");
            isNearbyWall = collision.gameObject;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == isNearbyWall)
        {
            if (isHuggingWall)
            {
                print("Applying magnetic force");

                // First, offset gravity
                rigidbody2d.AddForce(-1 * Physics2D.gravity * rigidbody2d.gravityScale, ForceMode2D.Force);

                if (rigidbody2d.velocity.magnitude < 0.1)
                {
                    // Normalize low velocity
                    rigidbody2d.velocity = Vector2.zero;
                }
                else if (rigidbody2d.velocity.y > 0)
                {
                    // Stop residual upwards velocity, "stick" to the wall
                    rigidbody2d.velocity = Vector2.zero;
                }
                else if (rigidbody2d.velocity.y < 0)
                {
                    // This is the "friction" force pushing up on the player
                    rigidbody2d.AddForce(Vector2.up * rigidbody2d.velocity.y * -1 * rigidbody2d.gravityScale * wallHugForceMultiplier);
                }
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WallJumpable")
        {
            print("Left jumpable wall");
            isNearbyWall = null;
        }
    }
}
