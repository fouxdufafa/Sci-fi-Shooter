using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class RobotBoyCharacter : MonoBehaviour, IDamageable {

    public Vector2 Gravity = new Vector2(0, -40f);
    public float MaxHorizontalSpeed = 15f;
    public float MaxVerticalSpeed = 10f;
    public float DashSpeedMultiplier = 1.2f;
    public float DashDuration = 0.3f;
    public float JumpSpeed = 20f;
    public float WallJumpSpeedMultiplier = 1.0f;
    public float WallJumpDuration = 0.1f;
    public float HurtLossOfControlDuration = 0.25f;
    public float InvulnerableDuration = 3f;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip hurtSound;
    public GameObject hookshotPrefab;

    CharacterController2D controller;
    public PlayerInput input { get; private set; }
    public Animator animator { get; private set; }
    public WallCheck wallCheck { get; private set; }
    public CollisionAwareStateMachine sm { get; private set; }
    public Hookshot hookshotInstance { get; private set; }
    Vector2 currentVelocity;
    Vector2 aimDirection;
    WeaponSystemV2 weaponSystem;
    AudioSource audioSource;
    DamageReceiver damageReceiver;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController2D>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        wallCheck = GetComponentInChildren<WallCheck>();
        weaponSystem = GetComponent<WeaponSystemV2>();
        audioSource = GetComponent<AudioSource>();
        damageReceiver = GetComponent<DamageReceiver>();
        currentVelocity = Vector2.zero;

        sm = new CollisionAwareStateMachine();
        controller.onControllerCollidedEvent += CollisionHandler;
        sm.ChangeState(new AirborneState(this));
	}

    private void Update()
    {
        sm.Update();
    }

    public void Move()
    {
        ClampVelocity();
        controller.move(currentVelocity * Time.deltaTime);
        UpdateWeaponsTransform();
    }

    public void ClampVelocity()
    {
        if (Mathf.Abs(currentVelocity.y) > MaxVerticalSpeed)
        {
            currentVelocity.y = Mathf.Sign(currentVelocity.y) * MaxVerticalSpeed;
        }
    }

    public void Jump()
    {
        SetVerticalVelocity(JumpSpeed);
        audioSource.PlayOneShot(jumpSound);
    }

    public void WallJump(Vector2 direction)
    {
        SetVelocity(direction.normalized * JumpSpeed * WallJumpSpeedMultiplier);
        audioSource.PlayOneShot(jumpSound);
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
        controller.move(new Vector2(0, -0.001f));
        return controller.isGrounded;
    }

    public bool IsTouchingCeiling()
    {
        controller.move(new Vector2(0, 0.001f));
        bool isTouching = controller.isTouchingCeiling;
        controller.move(new Vector2(0, -0.001f));

        return isTouching;
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

    public void SetAimDirection(float x, float y, bool ignoreMaxDelta = false)
    {
        weaponSystem.SetAimDirection(new Vector2(x, y), ignoreMaxDelta);
    }

    public void EnableCrosshair()
    {
        weaponSystem.EnableCrosshair();
    }

    public void DisableCrosshair()
    {
        weaponSystem.DisableCrosshair();
    }

    public void AimAndFaceCrosshair()
    {
        float horizontal = input.HorizontalAim.Value;
        float vertical = input.VerticalAim.Value;
        FaceTowards(horizontal);
        SetAimDirection(horizontal, vertical);
    }

    public void StopAim()
    {
        DisableCrosshair();
        SetAimDirection(GetFacing());
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

    public void TakeDamage(Damager damager)
    {
        audioSource.PlayOneShot(hurtSound);
        sm.ChangeState(new HurtState(this, damager));
    }

    public void Knockback(Vector2 direction, float speed)
    {
        SetVelocity(direction * speed);
    }

    public IEnumerator MakeInvulnerable(float duration)
    {
        damageReceiver.enabled = false;
        yield return new WaitForSeconds(duration);
        damageReceiver.enabled = true;
    }

    public void FireHookshot()
    {
        // instantiate hookshot object
        // set direction 
        if (hookshotInstance == null)
        {
            Debug.Log("Firing hookshot!");
            hookshotInstance = Instantiate(hookshotPrefab).GetComponent<Hookshot>();
            hookshotInstance.SetPosition(weaponSystem.WeaponSocket.position);
            hookshotInstance.SetRotation(weaponSystem.WeaponSocket.rotation);
        }
    }

    public void AttachToHookshot(Hookshot hookshot)
    {
        sm.ChangeState(new HookshotAttachedState(this));
    }

    public void DetachFromHookshot(Hookshot hookshot)
    {
        sm.ChangeState(new AirborneState(this));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2) transform.position + (weaponSystem != null ? weaponSystem.AimDirection : (Vector2) transform.right));
    }

    void CollisionHandler(RaycastHit2D hit)
    {
        sm.HandleCollision(hit.collider);
    }
}
