using UnityEngine;
using System.Collections;
using Prime31;

public class AirborneState : IState
{
    RobotBoyCharacter character;
    PlayerInput input;
    WallCheck wallCheck;
    Animator animator;
    StateMachine sm;

    public AirborneState(RobotBoyCharacter character)
    {
        this.character = character;
        this.input = character.input;
        this.animator = character.animator;
        this.wallCheck = character.wallCheck;
        this.sm = character.sm;
    }

    public void Enter()
    {
        animator.SetBool("Ground", false);
        if (input.WallHug.Held)
        {
            if (wallCheck.Contact != null)
            {
                sm.ChangeState(new WallHuggingState(character));
                return;
            }
        }
        if (input.Aim.Held)
        {
            sm.ChangeState(new AirborneAimingState(character));
            return;
        }
    }

    public void Update()
    {
        if (input.WallHug.Held)
        {
            if (wallCheck.Contact != null)
            {
                sm.ChangeState(new WallHuggingState(character));
                return;
            }
        }

        // TODO: Set vertical velocity to zero if we hit the ceiling

        character.ApplyGravity();
        character.SetHorizontalVelocity(character.MaxHorizontalSpeed * input.HorizontalMovement.Value);
        character.FaceTowardsVelocity();
        character.Move();
        character.SetAimDirection(character.GetFacing(), true);

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

        if (input.HookShot.Down)
        {
            character.FireHookshot();
        }

        if (character.IsTouchingCeiling())
        {
            character.SetVerticalVelocity(0);
        }

        if (character.IsGrounded())
        {
            sm.ChangeState(new GroundedState(character));
            return;
        }

        if (input.Dash.Down)
        {
            Debug.Log("Roll pressed in " + this);
            sm.ChangeState(new DashingState(character));
            return;
        }

        if (input.Jump.Down)
        {
            if (wallCheck.Contact != null)
            {
                sm.ChangeState(new WallJumpingState(character));
                return;
            }
        }

        if (input.Aim.Held)
        {
            sm.ChangeState(new AirborneAimingState(character));
            return;
        }
    }

    public void Exit()
    {

    }
}
