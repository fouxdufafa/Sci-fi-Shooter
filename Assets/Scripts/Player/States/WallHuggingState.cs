using UnityEngine;
using System.Collections;

public class WallHuggingState : IState
{
    RobotBoyCharacter character;
    PlayerInput input;
    Animator animator;
    WallCheck wallCheck;
    StateMachine sm;

    WallCheck.WallContact activeWall;

    // Use this for initialization
    public WallHuggingState(RobotBoyCharacter character)
    {
        this.character = character;
        this.input = character.input;
        this.animator = character.animator;
        this.wallCheck = character.wallCheck;
        this.sm = character.sm;
    }

    public void Enter()
    {
        character.SetVerticalVelocity(0);
        activeWall = wallCheck.Contact;

        character.EnableCrosshair();
        character.AimAndFaceCrosshair();
    }

    // Update is called once per frame
    public void Update()
    {
        // handle facing from joystick
        // check for state exits - trigger release, roll, etc
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

        if (input.WallHug.Value == 0)
        {
            // Release wall
            sm.ChangeState(new AirborneState(character));
            return;
        }
        if (input.Jump.Down)
        {
            sm.ChangeState(new WallJumpingState(character));
            return;
        }
        if (input.Dash.Down)
        {
            sm.ChangeState(new DashingState(character));
            return;
        }
    }

    public void Exit()
    {
        character.DisableCrosshair();
        character.StopAim();
    }
}
