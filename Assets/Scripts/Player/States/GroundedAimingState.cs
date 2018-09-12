using UnityEngine;
using System.Collections;

public class GroundedAimingState : IState
{
    RobotBoyCharacter character;
    PlayerInput input;
    Animator animator;
    StateMachine sm;

    // Use this for initialization
    public GroundedAimingState(RobotBoyCharacter character)
    {
        this.character = character;
        this.input = character.input;
        this.animator = character.animator;
        this.sm = character.sm;
    }

    public void Enter()
    {
        character.SetHorizontalVelocity(0f);
        character.EnableCrosshair();
        character.AimAndFaceCrosshair();
        animator.SetFloat("Speed", 0);
    }

    public void Update()
    {
        character.AimAndFaceCrosshair();

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

        if (input.Aim.Value == 0)
        {
            sm.ChangeState(new GroundedState(character));
            return;
        }
    }

    public void Exit()
    {
        character.DisableCrosshair();
        character.StopAim();
    }
}
