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
    Vector2 aimDirection;
    WeaponSystemV2 weaponSystem;

	// Use this for initialization
	void Start () {
        character = GetComponent<CharacterController2D>();
        weaponSystem = GetComponent<WeaponSystemV2>();
        currentVelocity = Vector2.zero;
	}

    public void Move()
    {
        character.move(currentVelocity * Time.deltaTime);
        UpdateWeaponsTransform();
    }

    public void Jump()
    {
        SetVerticalVelocity(JumpSpeed);
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

    public void FaceTowards(Vector2 direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(direction.x) * scale.x;
        transform.localScale = scale;
    }

    public void FaceTowards(float xDirection)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(xDirection) * Mathf.Abs(scale.x);
        transform.localScale = scale;
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

    public Vector2 GetFacing()
    {
        return transform.localScale.x > 0 ? Vector2.right : Vector2.left;
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

    public void SetAimDirection(Vector2 direction, bool ignoreMaxDelta = false)
    {
        weaponSystem.SetAimDirection(direction, ignoreMaxDelta);
    }

    public void EnableCrosshair()
    {
        weaponSystem.EnableCrosshair();
    }

    public void DisableCrosshair()
    {
        weaponSystem.DisableCrosshair();
    }

    public void FireWeapon()
    {
        weaponSystem.OnFirePressed();
    }

    public void ReleaseWeapon()
    {
        weaponSystem.OnFireReleased();
    }

    public void CycleWeapon()
    {
        weaponSystem.CycleWeapon();
    }

    public void UpdateWeaponsTransform()
    {
        weaponSystem.UpdateTransform(transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2) transform.position + weaponSystem.AimDirection);
    }
}
