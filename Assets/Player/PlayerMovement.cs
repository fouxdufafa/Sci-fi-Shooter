using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float jumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField] private LayerMask whatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded

    // Weapons
    [SerializeField] AudioClip shootSound;
    private AudioSource source;
    WeaponSystem weaponSystem;
    PlayerAim playerAim;
    [SerializeField] AudioClip cycleWeaponSound;


    // Movement
    private bool isGrounded;
    private bool isFacingRight = true;
    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    private Animator animator;            // Reference to the player's animator component.
    private Rigidbody2D rb2d;
    private float gravityScale;

    // Wall mechanics
    WallCheck wallGrabCheck;
    WallCheck wallJumpCheck;
    GameObject activeWall;
    FixedJoint2D joint;
    float lastWallJumpTime = 0f;
    float wallJumpDuration = 0.1f;

    // Roll mechanics
    float lastRollTime = -1f;
    float rollDuration = 0.3f;
    float rollSpeed = 14f;
    
    private void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("GroundCheck");
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        wallGrabCheck = transform.Find("WallGrabCheck").GetComponent<WallCheck>();
        wallJumpCheck = transform.Find("WallJumpCheck").GetComponent<WallCheck>();
        activeWall = null;
        gravityScale = rb2d.gravityScale; // save gravity scale

        // Weapons
        weaponSystem = GetComponent<WeaponSystem>();
        playerAim = GetComponent<PlayerAim>();
        source = GetComponent<AudioSource>();

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

    public void Move(float horizontal, float vertical, bool jumpPressed, TriggerState leftTrigger, float horizontalAim, float verticalAim, bool firePressed, bool fireReleased, bool rollPressed, bool cycleWeaponPressed)
    {
        bool isWallJumping = Time.fixedTime - lastWallJumpTime <= wallJumpDuration;

        bool isRolling = Time.fixedTime - lastRollTime <= rollDuration;
        animator.SetBool("Roll", isRolling);

        bool shouldRoll = rollPressed;

        bool shouldFire = firePressed && !isRolling;

        bool shouldStopFire = fireReleased;

        bool shouldCycleWeapon = cycleWeaponPressed;

        bool isAiming = (leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay) && (isGrounded || activeWall) && !isRolling;

        bool shouldMove = !isAiming && !activeWall && !isWallJumping && !isRolling && !shouldRoll;

        bool shouldJump = jumpPressed && !isAiming && isGrounded && animator.GetBool("Ground");

        bool shouldWallJump = jumpPressed && wallJumpCheck.Contact != null && !isGrounded;

        bool shouldStartHuggingWall = (!isGrounded && wallGrabCheck.Contact != null && !activeWall && (leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay) && !isWallJumping && !isRolling);

        bool shouldReleaseWall = (activeWall && !((leftTrigger == TriggerState.Start || leftTrigger == TriggerState.Stay)) || shouldWallJump || shouldRoll);


        if (shouldStartHuggingWall)
        {
            WallCheck.WallContact contact = wallGrabCheck.Contact;
            // Start hugging a wall
            activeWall = contact.Wall;
            joint.enabled = true;
            joint.connectedBody = contact.Wall.GetComponentInParent<Rigidbody2D>();
        }
        else if (shouldReleaseWall)
        {
            activeWall = null;
            joint.enabled = false;
            joint.connectedBody = null;
        }

        // Handle aiming / crosshair rendering
        if (isAiming)
        {
            playerAim.Aim(new Vector2(horizontalAim, verticalAim), true);
            if (isGrounded)
            {
                // Prevent sliding while aiming
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                animator.SetFloat("Speed", 0f);
            }
        }
        else
        {
            playerAim.Aim(isFacingRight ? Vector2.right : Vector2.left, false);
        }

        if (shouldFire)
        {
            weaponSystem.OnFirePressed();
        }
        
        if (shouldStopFire)
        {
            weaponSystem.OnFireReleased();
        }

        if (shouldCycleWeapon)
        {
            weaponSystem.CycleWeapon();
            GetComponent<AudioSource>().PlayOneShot(cycleWeaponSound);
        }

        if (shouldRoll)
        {
            lastRollTime = Time.fixedTime;
            isRolling = true;
            animator.SetBool("Roll", true);
            // set rolling velocity
            rb2d.velocity = new Vector2(transform.localScale.x * rollSpeed, rb2d.velocity.y);
        }

        // Fire of a jump if we should
        if (shouldJump)
        {
            // Add a vertical force to the player.
            isGrounded = false;
            animator.SetBool("Ground", false);
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(0f, jumpForce));
        }

        if (shouldWallJump)
        {
            // add upwards diagonal force away from the wall
            WallCheck.WallContact contact = wallJumpCheck.Contact;
            Vector2 contactPoint = contact.ContactPoint;
            Vector2 jumpDirection;

            if (transform.position.x < contactPoint.x)
            {
                // wall is on the right, push up and left
                jumpDirection = (new Vector2(-1, 1)).normalized;
            }
            else
            {
                // wall is on the left, push up and right
                jumpDirection = (new Vector2(1, 1)).normalized;
            }
            rb2d.velocity = new Vector2(0, 0);
            rb2d.AddForce(jumpDirection * jumpForce * 1.5f);

            lastWallJumpTime = Time.fixedTime;
        }

        // Handle movement.
        if (shouldMove)
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

        // Finally, face the player in the correct direction.
        if (horizontal > 0 && !isFacingRight && !isRolling)
        {
            Flip();
        }
        else if (horizontal < 0 && isFacingRight && !isRolling)
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
        if (activeWall)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + new Vector3(joint.anchor.x, joint.anchor.y), 0.1f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y), 0.1f);
        }
    }
}
