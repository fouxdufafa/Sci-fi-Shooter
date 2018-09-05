using UnityEngine;
using System.Collections;
using Prime31;

public class AirborneSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    PlayerInput input;
    WallCheck wallHugCheck;
    Animator animator;

    public void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
        input = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        wallHugCheck = GetComponentInChildren<WallCheck>();
    }

    public override void OnEnter(StateMachine sm)
    {
        animator.SetBool("Ground", false);
    }

    public override void OnUpdate(StateMachine sm)
    {
        if (input.WallHug.Value == 1)
        {
            if (wallHugCheck.Contact != null)
            {
                sm.TransitionTo<WallHugSMB>();
                return;
            }
        }

        character.ApplyGravity();
        character.SetHorizontalVelocity(character.MaxHorizontalSpeed * Input.GetAxis("Horizontal"));
        character.FaceTowardsVelocity();
        character.Move();

        if (character.IsGrounded())
        {
            sm.TransitionTo<GroundedSMB>();
            return;
        }

        if (input.Dash.Down)
        {
            Debug.Log("Roll pressed in " + this);
            sm.TransitionTo<DashingSMB>();
            return;
        }

        if (input.Jump.Down)
        {
            if (wallHugCheck.Contact != null)
            {
                sm.TransitionTo<WallJumpSMB>();
                return;
            }
        }
    }
}
