using UnityEngine;
using System.Collections;

public class WallHugSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    PlayerInput input;
    WallCheck wallCheck;

    WallCheck.WallContact activeWall;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
        input = GetComponent<PlayerInput>();
        wallCheck = GetComponentInChildren<WallCheck>();
    }

    public override void OnEnter(StateMachine sm)
    {
        character.SetVerticalVelocity(0);
        activeWall = wallCheck.Contact;

        float horizontal = input.HorizontalAim.Value;
        float vertical = input.VerticalAim.Value;
        character.SetAimDirection(new Vector2(horizontal, vertical));
        character.EnableCrosshair();
    }

    // Update is called once per frame
    public override void OnUpdate(StateMachine sm)
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
            sm.TransitionTo<AirborneSMB>();
            return;
        }
        if (input.Jump.Down)
        {
            sm.TransitionTo<WallJumpSMB>();
            return;
        }
        if (input.Dash.Down)
        {
            sm.TransitionTo<DashingSMB>();
            return;
        }
    }

    public override void OnExit(StateMachine sm)
    {
        character.SetAimDirection(transform.forward);
        character.DisableCrosshair();
    }
}
