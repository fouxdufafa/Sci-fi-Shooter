﻿using UnityEngine;
using System.Collections;

public class DashingState : IState
{
    RobotBoyCharacter character;
    Animator animator;
    PlayerInput input;
    StateMachine sm;

    public bool IgnoreVerticalVelocity = false;

    Coroutine waitForDash;

    // Use this for initialization
    public DashingState(RobotBoyCharacter character)
    {
        this.character = character;
        this.input = character.input;
        this.animator = character.animator;
        this.sm = character.sm;
    }

    public void Enter()
    {
        animator.SetBool("Roll", true);
        character.SetHorizontalVelocity(character.MaxHorizontalSpeed * character.DashSpeedMultiplier * Mathf.Sign(character.transform.localScale.x));
        if (IgnoreVerticalVelocity)
        {
            character.SetVerticalVelocity(0);
        }
        character.ReleaseWeapon();
        waitForDash = character.StartCoroutine(WaitForDashComplete());
    }

    public void Update()
    {
        character.Move();
        if (!character.IsGrounded() && !IgnoreVerticalVelocity)
        {
            character.ApplyGravity();
        }
        if (input.CycleWeapon.Down)
        {
            character.CycleWeapon();
        }
    }

    public void Exit()
    {
        character.StopCoroutine(waitForDash);
        animator.SetBool("Roll", false);
    }

    IEnumerator WaitForDashComplete()
    {
        yield return new WaitForSeconds(character.DashDuration);
        if (character.IsGrounded())
        {
            sm.ChangeState(new GroundedState(character));
        }
        else
        {
            sm.ChangeState(new AirborneState(character));
        }
    }
}
