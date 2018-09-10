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
    public WallJumpingState(RobotBoyCharacter character, PlayerInput input, Animator animator, WallCheck wallCheck, StateMachine sm)
    {
        this.character = character;
        this.input = input;
        this.animator = animator;
        this.wallCheck = wallCheck;
        this.sm = sm;
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
    }

    public void Update()
    {
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
    }

    public void Exit()
    {
        character.StopCoroutine(waitForWallJump);
    }

    IEnumerator WaitForJumpComplete()
    {
        yield return new WaitForSeconds(character.WallJumpDuration);
        sm.ChangeState(new AirborneState(character, input, animator, wallCheck, sm));
    }
}
