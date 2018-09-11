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
    public WallHuggingState(RobotBoyCharacter character, PlayerInput input, Animator animator, WallCheck wallCheck, StateMachine sm)
    {
        this.character = character;
        this.input = input;
        this.animator = animator;
        this.wallCheck = wallCheck;
        this.sm = sm;
    }

    public void Enter()
    {
        character.SetVerticalVelocity(0);
        activeWall = wallCheck.Contact;

        float horizontal = input.HorizontalAim.Value;
        float vertical = input.VerticalAim.Value;
        character.SetAimDirection(new Vector2(horizontal, vertical));
        character.EnableCrosshair();
    }

    // Update is called once per frame
    public void Update()
    {
        // handle facing from joystick
        // check for state exits - trigger release, roll, etc
        character.FaceTowards(Input.GetAxis("Horizontal"));

        float horizontal = input.HorizontalAim.Value;
        float vertical = input.VerticalAim.Value;
        character.SetAimDirection(new Vector2(horizontal, vertical));

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

        if (input.WallHug.Value == 0)
        {
            // Release wall
            sm.ChangeState(new AirborneState(character, input, animator, wallCheck, sm));
            return;
        }
        if (input.Jump.Down)
        {
            sm.ChangeState(new WallJumpingState(character, input, animator, wallCheck, sm));
            return;
        }
        if (input.Dash.Down)
        {
            sm.ChangeState(new DashingState(character, input, animator, sm));
            return;
        }
    }

    public void Exit()
    {
        character.SetAimDirection(character.transform.forward);
        character.DisableCrosshair();
    }
}
