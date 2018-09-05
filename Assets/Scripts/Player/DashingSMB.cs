using UnityEngine;
using System.Collections;

public class DashingSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    Animator animator;

    public bool IgnoreVerticalVelocity = false;

    Coroutine waitForDash;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnEnter(StateMachine sm)
    {
        animator.SetBool("Roll", true);
        character.SetHorizontalVelocity(character.MaxHorizontalSpeed * character.DashSpeedMultiplier * Mathf.Sign(transform.localScale.x));
        if (IgnoreVerticalVelocity)
        {
            character.SetVerticalVelocity(0);
        }
        waitForDash = StartCoroutine(WaitForDashComplete(sm));
    }

    // Update is called once per frame
    public override void OnUpdate(StateMachine sm)
    {
        character.Move();
        if (!character.IsGrounded() && !IgnoreVerticalVelocity)
        {
            character.ApplyGravity();
        }
    }

    public override void OnExit(StateMachine sm)
    {
        animator.SetBool("Roll", false);
    }

    IEnumerator WaitForDashComplete(StateMachine sm)
    {
        yield return new WaitForSeconds(character.DashDuration);
        if (character.IsGrounded())
        {
            sm.TransitionTo<GroundedSMB>();
        }
        else
        {
            sm.TransitionTo<AirborneSMB>();
        }
    }
}
