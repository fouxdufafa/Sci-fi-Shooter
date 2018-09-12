using UnityEngine;
using System.Collections;

public class WallJumpingState : IState
{
    RobotBoyCharacter character;
    PlayerInput input;
    Animator animator;
    WallCheck wallCheck;
    StateMachine sm;

    Coroutine waitForWallJump;

    // Use this for initialization
    public WallJumpingState(RobotBoyCharacter character)
    {
        this.character = character;
        this.input = character.input;
        this.animator = character.animator;
        this.wallCheck = character.wallCheck;
        this.sm = character.sm;
    }

    public void Enter()
    {
        WallCheck.WallContact contact = wallCheck.Contact;
        Vector2 contactPoint = contact.ContactPoint;
        Vector2 jumpDirection;

        if (character.transform.position.x < contactPoint.x)
        {
            // wall is on the right, push up and left
            jumpDirection = (new Vector2(-1, 1)).normalized;
        }
        else
        {
            // wall is on the left, push up and right
            jumpDirection = (new Vector2(1, 1)).normalized;
        }

        character.WallJump(jumpDirection);

        waitForWallJump = character.StartCoroutine(WaitForJumpComplete());

        if (input.Aim.Held)
        {
            character.EnableCrosshair();
            character.AimAndFaceCrosshair();
        }
    }

    public void Update()
    {
        character.Move();

        if (input.Aim.Held)
        {
            character.EnableCrosshair();
            character.AimAndFaceCrosshair();
        } else
        {
            character.DisableCrosshair();
            character.StopAim();
        }

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
    }

    public void Exit()
    {
        character.StopCoroutine(waitForWallJump);
    }

    IEnumerator WaitForJumpComplete()
    {
        yield return new WaitForSeconds(character.WallJumpDuration);
        sm.ChangeState(new AirborneState(character));
    }
}
