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
    }

    // Update is called once per frame
    public override void OnUpdate(StateMachine sm)
    {
        // handle facing from joystick
        // check for state exits - trigger release, roll, etc
        character.FaceTowards(Input.GetAxis("Horizontal"));

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
}
