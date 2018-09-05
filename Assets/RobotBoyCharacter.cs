using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class RobotBoyCharacter : MonoBehaviour {

    public Vector2 Gravity = new Vector2(0, -40f);
    public float MaxHorizontalSpeed = 15f;
    public float DashSpeedMultiplier = 1.2f;
    public float DashDuration = 0.3f;
    public float JumpSpeed = 20f;

    CharacterController2D character;
    Vector2 currentVelocity;

    bool isFacingRight = true;

	// Use this for initialization
	void Start () {
        character = GetComponent<CharacterController2D>();
        currentVelocity = Vector2.zero;
	}

    public void Move()
    {
        character.move(currentVelocity * Time.deltaTime);
    }

    public void FaceTowardsVelocity()
    {
        if (currentVelocity.x > 0)
        {
            FaceRight();
        }
        if (currentVelocity.x < 0)
        {
            FaceLeft();
        }
    }

    public void FaceLeft()
    {
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void FaceRight()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public bool IsGrounded()
    {
        character.move(new Vector2(0, -0.001f));
        return character.isGrounded;
    }

    public void ApplyGravity()
    {
        currentVelocity += Gravity * Time.deltaTime;
    }

    public void SetVelocity(Vector2 velocity)
    {
        currentVelocity = velocity;
    }

    public void SetVerticalVelocity(float v)
    {
        currentVelocity.y = v;
    }

    public void SetHorizontalVelocity(float v)
    {
        currentVelocity.x = v;
    }
}
