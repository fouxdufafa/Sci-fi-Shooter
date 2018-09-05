using UnityEngine;
using System.Collections;
using Prime31;

public class AirborneSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    Animator animator;

    public void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnEnter(StateMachine sm)
    {
        animator.SetBool("Ground", false);
    }

    public override void OnUpdate(StateMachine sm)
    {
        character.ApplyGravity();
        character.SetHorizontalVelocity(character.MaxHorizontalSpeed * Input.GetAxis("Horizontal"));
        character.FaceTowardsVelocity();
        character.Move();

        if (character.IsGrounded())
        {
            sm.TransitionTo<GroundedSMB>();
            return;
        }

        if (Input.GetButtonDown("Roll"))
        {
            sm.TransitionTo<DashingSMB>();
            return;
        }
    }
}
