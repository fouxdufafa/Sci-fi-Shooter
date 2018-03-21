using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float jumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField] private LayerMask whatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded

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
    private bool isTakingDamage = false;

    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    private Animator animator;            // Reference to the player's animator component.
    private Rigidbody2D rb2d;
    private float gravityScale;

    WallCheck wallCheck;
    GameObject activeWall;
    FixedJoint2D joint;
    
    private void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("GroundCheck");
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        wallCheck = GetComponent<WallCheck>();
        activeWall = null;
        gravityScale = rb2d.gravityScale; // save gravity scale

        joint = GetComponent<FixedJoint2D>();
        joint.enabled = false;
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
        animator.SetFloat("vSpeed", rb2d.velocity.y);
    }

    public void TakeDamage(float amount, float stunSeconds)
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            StartCoroutine(TakeDamageCoroutine(stunSeconds));
        }
    }

    IEnumerator TakeDamageCoroutine(float stunSeconds)
    {
        yield return new WaitForSeconds(stunSeconds);
        isTakingDamage = false;
    }

    public void Move(float horizontal, float vertical, bool jumpPressed, TriggerState leftTrigger, float horizontalAim, float verticalAim, bool fire)
    {
        if (isTakingDamage)
        {
            animator.SetFloat("Speed", 0);
            crosshair.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }

        // Prevent rigidbody from sleeping in a stationary wall-hug state
        if (leftTrigger == TriggerState.End && rb2d.velocity.magnitude == 0)
        {
            rb2d.WakeUp();
        }

        bool isNearWall = wallCheck.Contact != null;

        bool shouldStartHuggingWall = (!isGrounded && isNearWall && !activeWall && (leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay));

        bool shouldReleaseWall = (activeWall && !((leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay)));

        // Determine if player is hugging a nearby wall
        isHuggingWall = (!isGrounded && isNearWall && (leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay));

        // Determine if player is aiming
        isAiming = (leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay) && (isGrounded || activeWall);

        // Determine if player can move (not aiming or wallhugging)
        bool canMove = !isAiming && !activeWall;

        // Determine if player should fire off a jump
        bool canJump = isGrounded && !isAiming && jumpPressed && animator.GetBool("Ground");

        if (shouldStartHuggingWall)
        {
            // Start hugging a wall
            WallCheck.WallContact contact = wallCheck.Contact;
            activeWall = contact.Wall;
            joint.enabled = true;
            joint.connectedBody = contact.Wall.GetComponentInParent<Rigidbody2D>();

            print("Anchor: " + joint.anchor);
            print("Connected: " + joint.connectedAnchor);
        }
        else if (shouldReleaseWall)
        {
            activeWall = null;
            joint.enabled = false;
            joint.connectedBody = null;
        }

        print(activeWall);

        // Handle wall hugging
        //if (isHuggingWall)
        //{
        //    print("Grabbing static wall");
        //    transform.SetParent(wallCheck.Contact.Wall.transform);
        //    rb2d.velocity = Vector2.zero;
        //    rb2d.gravityScale = 0;

        //}
        //else
        //{
        //    print("Releasing wall");
        //    transform.SetParent(null);
        //    rb2d.gravityScale = gravityScale;
        //}

        // Handle aiming / crosshair rendering
        if (isAiming)
        {
            Vector3 offset = new Vector2(horizontalAim, verticalAim).normalized * crosshairRadius;
            crosshair.GetComponent<SpriteRenderer>().enabled = offset.magnitude > 0;
            crosshair.transform.position = transform.position + offset;

            if (isGrounded)
            {
                // Prevent sliding while aiming
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
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
            rb2d.velocity = new Vector2(horizontal * maxSpeed, rb2d.velocity.y);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        if (fire)
        {
            Vector3 velocity;
            if (isAiming)
            {
                velocity = (crosshair.transform.position - projectileSocket.transform.position).normalized * projectileSpeed;
            }
            else
            {
                velocity = new Vector2(transform.localScale.normalized.x, 0) * projectileSpeed;
            }
            GameObject newProjectile = Instantiate(projectile, projectileSocket.transform.position, Quaternion.FromToRotation(Vector2.right, velocity.normalized));
            newProjectile.transform.rotation = Quaternion.FromToRotation(Vector2.right, velocity.normalized);
            newProjectile.GetComponent<Rigidbody2D>().velocity = velocity;
        }

        // Fire of a jump if we should
        if (canJump)
        {
            // Add a vertical force to the player.
            isGrounded = false;
            print("Jumped");
            animator.SetBool("Ground", false);
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(0f, jumpForce));
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

        // Also reposition the joint.
        joint.anchor *= -1;
    }

    private void OnDrawGizmos()
    {
        if (isAiming)
        {
            //Gizmos.DrawLine(projectileSocket.transform.position, crosshair.transform.position);
        }

        if (activeWall)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + new Vector3(joint.anchor.x, joint.anchor.y), 0.1f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y), 0.1f);
        }
    }
}
