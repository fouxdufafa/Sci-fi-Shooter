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

    public AirborneState(RobotBoyCharacter character, PlayerInput input, Animator animator, WallCheck wallHugCheck, StateMachine sm)
    {
        this.character = character;
        this.input = input;
        this.animator = animator;
        this.wallCheck = wallHugCheck;
        this.sm = sm;
    }

    public void Enter()
    {
        animator.SetBool("Ground", false);
    }

    public void Update()
    {
        if (input.WallHug.Value == 1)
        {
            if (wallCheck.Contact != null)
            {
                sm.ChangeState(new WallHuggingState(character, input, animator, wallCheck, sm));
                return;
            }
        }

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

        if (character.IsGrounded())
        {
            sm.ChangeState(new GroundedState(character, input, animator, sm));
            return;
        }

        if (input.Dash.Down)
        {
            Debug.Log("Roll pressed in " + this);
            sm.ChangeState(new DashingState(character, input, animator, sm));
            return;
        }

        if (input.Jump.Down)
        {
            if (wallCheck.Contact != null)
            {
                sm.ChangeState(new WallJumpingState(character, input, animator, wallCheck, sm));
                return;
            }
        }
    }

    public void Exit()
    {

    }
}
