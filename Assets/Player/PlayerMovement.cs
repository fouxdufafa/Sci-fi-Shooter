using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float jumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField] private LayerMask whatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] float wallHugForceMultiplier = 3f;

    // Crosshair
    [SerializeField] GameObject crosshair;
    [SerializeField] float crosshairRadius;

    // Projectile
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 100f;
    [SerializeField] GameObject projectileSocket;

    // State / position flags
    private bool isGrounded;
    private bool isAiming = false;
    private bool isHuggingWall = false;
    private bool isFacingRight = true;

    // Adjacent wall, if any
    private GameObject adjacentWall = null;

    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private Animator animator;            // Reference to the player's animator component.
    private Rigidbody2D rigidbody2d;
    
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

    public void Move(float horizontal, float vertical, bool jump, TriggerState leftTrigger, float horizontalAim, float verticalAim, bool fire)
    {
        // Prevent rigidbody from sleeping in a stationary wall-hug state
        if (leftTrigger == TriggerState.End && rigidbody2d.velocity.magnitude == 0)
        {
            rigidbody2d.WakeUp();
        }

        // Determine if player is hugging a nearby wall
        isHuggingWall = (!isGrounded && adjacentWall && (leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay));

        // Determine if player is aiming
        isAiming = (leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay) && (isGrounded || isHuggingWall);

        // Determine if player can move (not aiming or wallhugging)
        bool canMove = !isAiming && !isHuggingWall;

        // Determine if player should fire off a jump
        bool canJump = isGrounded && !isAiming && jump && animator.GetBool("Ground");


        // Handle wall hugging
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

        // Handle aiming / crosshair rendering
        if (isAiming)
        {
            Vector3 offset = new Vector2(horizontalAim, verticalAim).normalized * crosshairRadius;
            crosshair.GetComponent<SpriteRenderer>().enabled = offset.magnitude > 0;
            crosshair.transform.position = transform.position + offset;

            if (isGrounded)
            {
                // Prevent sliding while aiming
                rigidbody2d.velocity = Vector2.zero;
                animator.SetFloat("Speed", 0f);
            }
        }
        else
        {
            crosshair.GetComponent<SpriteRenderer>().enabled = false;
        }


        // Handle movement.
        if (canMove)
        {
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            animator.SetFloat("Speed", Mathf.Abs(horizontal));

            // Move the character
            rigidbody2d.velocity = new Vector2(horizontal * maxSpeed, rigidbody2d.velocity.y);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        if (fire)
        {
            GameObject newProjectile = Instantiate(projectile, projectileSocket.transform.position, Quaternion.identity);
            Vector3 velocity;

            if (isAiming)
            {
                velocity = (crosshair.transform.position - projectileSocket.transform.position).normalized * projectileSpeed;
            }
            else
            {
                velocity = new Vector2(transform.localScale.normalized.x, 0) * projectileSpeed;
            }

            newProjectile.GetComponent<Rigidbody>().velocity = velocity;
        }

        // Fire of a jump if we should
        if (canJump)
        {
            // Add a vertical force to the player.
            isGrounded = false;
            print("Jumped");
            animator.SetBool("Ground", false);
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
            rigidbody2d.AddForce(new Vector2(0f, jumpForce));
        }

        // Finally, face the player in the correct direction.
        if (horizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && isFacingRight)
        {
            Flip();
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
        if (collision.gameObject.CompareTag("WallJumpable"))
        {
            print("Hit jumpable wall");
            adjacentWall = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WallJumpable"))
        {
            print("Left jumpable wall");
            adjacentWall = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (isAiming)
        {
            Gizmos.DrawLine(projectileSocket.transform.position, crosshair.transform.position);
        }
        
    }
}
