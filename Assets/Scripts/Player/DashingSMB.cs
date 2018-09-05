using UnityEngine;
using System.Collections;

public class DashingSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    Animator animator;

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
        StartCoroutine(WaitForDashComplete(sm));
    }

    // Update is called once per frame
    public override void OnUpdate(StateMachine sm)
    {
        character.Move();
        if (!character.IsGrounded())
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
