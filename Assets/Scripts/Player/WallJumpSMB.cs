using UnityEngine;
using System.Collections;

public class WallJumpSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    PlayerInput input;
    WallCheck wallCheck;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
        input = GetComponent<PlayerInput>();
        wallCheck = GetComponentInChildren<WallCheck>();
    }

    public override void OnEnter(StateMachine sm)
    {
        WallCheck.WallContact contact = wallCheck.Contact;
        Vector2 contactPoint = contact.ContactPoint;
        Vector2 jumpDirection;

        if (transform.position.x < contactPoint.x)
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

        StartCoroutine(WaitForJumpComplete(sm));
    }

    public override void OnUpdate(StateMachine sm)
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

    IEnumerator WaitForJumpComplete(StateMachine sm)
    {
        yield return new WaitForSeconds(character.WallJumpDuration);
        sm.TransitionTo<AirborneSMB>();
    }
}
