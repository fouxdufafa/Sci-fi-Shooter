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
        float velocity = character.MaxHorizontalSpeed * input.HorizontalMovement.Value;
        character.SetHorizontalVelocity(velocity);
        character.FaceTowardsVelocity();
        character.Move();
        character.SetAimDirection(character.GetFacing(), true);

        animator.SetFloat("Speed", Mathf.Abs(velocity));

        if (!character.IsGrounded())
        {
            sm.TransitionTo<AirborneSMB>(); 
            return;
        }

        if (input.Fire.Down)
        {
            character.FireWeapon();
        }

        if (input.Fire.Up)
        {
            character.ReleaseWeapon();
        }

        if (input.CycleWeapon.Down)
        {
            character.CycleWeapon();
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
