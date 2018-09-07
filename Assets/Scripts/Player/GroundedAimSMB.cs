using UnityEngine;
using System.Collections;

public class GroundedAimSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    PlayerInput input;
    Animator animator;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    public override void OnEnter(StateMachine sm)
    {
        character.SetHorizontalVelocity(0f);
        character.EnableCrosshair();
        float horizontal = input.HorizontalAim.Value;
        float vertical = input.VerticalAim.Value;
        character.SetAimDirection(new Vector2(horizontal, vertical));
        animator.SetFloat("Speed", 0);
    }

    public override void OnUpdate(StateMachine sm)
    {
        float horizontal = input.HorizontalAim.Value;
        float vertical = input.VerticalAim.Value;
        character.FaceTowards(horizontal);
        character.SetAimDirection(new Vector2(horizontal, vertical));

        if (input.Fire.Down)
        {
            character.FireWeapon();
        }

        if (input.Fire.Up)
        {
            character.ReleaseWeapon();
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

        if (input.Aim.Value == 0)
        {
            sm.TransitionTo<GroundedSMB>();
            return;
        }
    }

    public override void OnExit(StateMachine sm)
    {
        character.DisableCrosshair();
        character.SetAimDirection(character.GetFacing());
    }
}
