using UnityEngine;
using System.Collections;

public class AirborneAimingState : IState
{
    RobotBoyCharacter character;
    PlayerInput input;
    WallCheck wallCheck;
    Animator animator;
    StateMachine sm;

    public AirborneAimingState(RobotBoyCharacter character)
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
        character.EnableCrosshair();
        character.AimAndFaceCrosshair();
    }

    public void Update()
    {
        if (input.WallHug.Value == 1)
        {
            if (wallCheck.Contact != null)
            {
                sm.ChangeState(new WallHuggingState(character));
                return;
            }
        }

        character.AimAndFaceCrosshair();

        // TODO: Set vertical velocity to zero if we hit the ceiling
        character.ApplyGravity();
        character.SetHorizontalVelocity(character.MaxHorizontalSpeed * input.HorizontalMovement.Value);
        character.Move();

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

        if (input.Aim.Up)
        {
            sm.ChangeState(new AirborneState(character));
            return;
        }
    }

    public void Exit()
    {
        character.DisableCrosshair();
    }
}

