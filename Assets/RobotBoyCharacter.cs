﻿using System;
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
    public float WallJumpSpeedMultiplier = 1.5f;
    public float WallJumpDuration = 0.1f;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public GameObject hookshotPrefab;

    CharacterController2D controller;
    PlayerInput input;
    Animator animator;
    [HideInInspector] public WallCheck wallCheck;
    StateMachine sm;
    Vector2 currentVelocity;
    Vector2 aimDirection;
    WeaponSystemV2 weaponSystem;
    Hookshot hookshotInstance;
    AudioSource audioSource;

    struct States
    {
        public GroundedState Grounded;
        public GroundedAimingState GroundedAim;
        public AirborneState Airborne;
        public DashingState Dashing;
        public WallHuggingState WallHug;
        public WallJumpingState WallJump;
    }

    public event Action<Damager> OnTakeDamage;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController2D>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        wallCheck = GetComponentInChildren<WallCheck>();
        weaponSystem = GetComponent<WeaponSystemV2>();
        audioSource = GetComponent<AudioSource>();
        currentVelocity = Vector2.zero;

        sm = new StateMachine();
        sm.ChangeState(new AirborneState(this, input, animator, wallCheck, sm));
	}

    private void Update()
    {
        sm.Update();
    }

    public void Move()
    {
        controller.move(currentVelocity * Time.deltaTime);
        UpdateWeaponsTransform();
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

    public void TakeDamage(Damager damager)
    {
        if (OnTakeDamage != null)
        {
            OnTakeDamage(damager);
        }
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
        sm.ChangeState(new HookshotAttachedState(this, sm, hookshotInstance));
    }

    public void DetachFromHookshot(Hookshot hookshot)
    {
        sm.ChangeState(new AirborneState(this, input, animator, wallCheck, sm));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2) transform.position + (weaponSystem != null ? weaponSystem.AimDirection : (Vector2) transform.right));
    }
}
