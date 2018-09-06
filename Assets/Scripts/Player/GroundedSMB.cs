using UnityEngine;
using System.Collections;

public class GroundedSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    PlayerInput input;
    Animator animator;

    public void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
        input = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnEnter(StateMachine sm)
    {
        character.SetVerticalVelocity(0f);
        animator.SetBool("Ground", true);

        // TODO: Instead of catching this in OnEnter, use a buffered input manager
        // so as not to miss button down events during state transitions
        if (input.Dash.Down)
        {
            sm.TransitionTo<DashingSMB>();
            return;
        }
    }

    // Update is called once per frame
    public override void OnUpdate(StateMachine sm)
    {
        float velocity = character.MaxHorizontalSpeed * Input.GetAxis("Horizontal");
        character.SetHorizontalVelocity(velocity);
        character.FaceTowardsVelocity();
        character.Move();

        animator.SetFloat("Speed", Mathf.Abs(velocity));

        if (!character.IsGrounded())
        {
            sm.TransitionTo<AirborneSMB>(); 
            return;
        }

        if (input.Jump.Down)
        {
            character.Jump();
            sm.TransitionTo<AirborneSMB>();
            return;
        }

        if (input.Dash.Down)
        {
            sm.TransitionTo<DashingSMB>();
            return;
        }

        if (input.Aim.Value == 1)
        {
            sm.TransitionTo<GroundedAimSMB>();
            return;
        }
    }
}
