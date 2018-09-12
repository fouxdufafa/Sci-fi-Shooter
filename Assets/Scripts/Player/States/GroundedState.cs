using UnityEngine;
using System.Collections;

public class GroundedState : IState
{
    RobotBoyCharacter character;
    PlayerInput input;
    Animator animator;
    StateMachine sm;

    public GroundedState(RobotBoyCharacter character)
    {
        this.character = character;
        this.input = character.input;
        this.animator = character.animator;
        this.sm = character.sm;
    }

    public void Enter()
    {
        character.SetVerticalVelocity(0f);
        animator.SetBool("Ground", true);

        if (input.Aim.Held)
        {
            sm.ChangeState(new GroundedAimingState(character));
            return;
        }

        // TODO: Instead of catching this in OnEnter, use a buffered input manager
        // so as not to miss button down events during state transitions
        if (input.Dash.Down)
        {
            sm.ChangeState(new DashingState(character));
            return;
        }
    }

    // Update is called once per frame
    public void Update() 
    {
        float velocity = character.MaxHorizontalSpeed * input.HorizontalMovement.Value;
        character.SetHorizontalVelocity(velocity);
        character.FaceTowardsVelocity();
        character.Move();
        character.SetAimDirection(character.GetFacing(), true);

        animator.SetFloat("Speed", Mathf.Abs(velocity));

        if (!character.IsGrounded())
        {
            sm.ChangeState(new AirborneState(character));
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
            sm.ChangeState(new AirborneState(character));
            return;
        }

        if (input.Dash.Down)
        {
            sm.ChangeState(new DashingState(character));
            return;
        }

        if (input.Aim.Held)
        {
            sm.ChangeState(new GroundedAimingState(character));
            return;
        }
    }

    public void Exit()
    {

    }
}
