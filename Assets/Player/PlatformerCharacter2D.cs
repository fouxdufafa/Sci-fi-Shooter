using System;
using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    private bool isHuggingWall = false;
    private GameObject nearbyWall = null;
    [SerializeField] float wallHugForceMultiplier = 3f;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }


    public void Move(float move, bool jump, WallHugButtonState wallHugButtonState)
    {
        // Prevent rigidbody from sleeping in a stationary wall-hug state
        if (wallHugButtonState == WallHugButtonState.End && m_Rigidbody2D.velocity.magnitude == 0)
        {
            m_Rigidbody2D.WakeUp();
        }

        // Determine if player is hugging a nearby wall
        if (!m_Grounded && nearbyWall)
        {
            if (wallHugButtonState == WallHugButtonState.Start || wallHugButtonState == WallHugButtonState.Remain)
            {
                isHuggingWall = true;
            }
            else
            {
                isHuggingWall = false;
            }
        }
        else
        {
            isHuggingWall = false;
        }

        //only move the player if grounded or not wall hugging
        if (m_Grounded || !isHuggingWall)
        {
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            if (!m_Grounded)
            {
                print("Moved player " + move * m_MaxSpeed);
            }

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

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
            nearbyWall = collision.gameObject;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == nearbyWall)
        {
            if (isHuggingWall)
            {
                print("Applying magnetic force");

                // First, offset gravity
                m_Rigidbody2D.AddForce(-1 * Physics2D.gravity * m_Rigidbody2D.gravityScale, ForceMode2D.Force);

                if (m_Rigidbody2D.velocity.magnitude < 0.1)
                {
                    // Normalize low velocity
                    m_Rigidbody2D.velocity = Vector2.zero;
                }
                else if (m_Rigidbody2D.velocity.y > 0)
                {
                    // Stop residual upwards velocity, "stick" to the wall
                    m_Rigidbody2D.velocity = Vector2.zero;
                }
                else if (m_Rigidbody2D.velocity.y < 0)
                {
                    // This is the "friction" force pushing up on the player
                    m_Rigidbody2D.AddForce(Vector2.up * m_Rigidbody2D.velocity.y * -1 * m_Rigidbody2D.gravityScale * wallHugForceMultiplier);
                }
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WallJumpable")
        {
            print("Left jumpable wall");
            nearbyWall = null;
        }
    }
}
