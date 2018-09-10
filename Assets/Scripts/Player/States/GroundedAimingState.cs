using UnityEngine;
using System.Collections;

public class GroundedAimingState : IState
{
    RobotBoyCharacter character;
    PlayerInput input;
    Animator animator;
    StateMachine sm;

    // Use this for initialization
    public GroundedAimingState(RobotBoyCharacter character, PlayerInput input, Animator animator, StateMachine sm)
    {
        this.character = character;
        this.input = input;
        this.animator = animator;
        this.sm = sm;
    }

    public void Enter()
    {
        character.SetHorizontalVelocity(0f);
        character.EnableCrosshair();
        float horizontal = input.HorizontalAim.Value;
        float vertical = input.VerticalAim.Value;
        character.SetAimDirection(new Vector2(horizontal, vertical));
        animator.SetFloat("Speed", 0);
    }

    public void Update()
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
        if (input.CycleWeapon.Down)
        {
            character.CycleWeapon();
        }

        if (input.Jump.Down)
        {
            character.Jump();
            sm.ChangeState(new AirborneState(character, input, animator, character.wallCheck, sm));
            return;
        }

        if (input.Dash.Down)
        {
            sm.ChangeState(new DashingState(character, input, animator, sm));
            return;
        }

        if (input.Aim.Value == 0)
        {
            sm.ChangeState(new GroundedState(character, input, animator, sm));
            return;
        }
    }

    public void Exit()
    {
        character.DisableCrosshair();
        character.SetAimDirection(character.GetFacing());
    }
}
