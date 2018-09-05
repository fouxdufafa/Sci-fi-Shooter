using UnityEngine;
using System.Collections;

public class WallJumpSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    WallCheck wallCheck;

    public float WallJumpDuration = 0.1f;
    public float WallJumpSpeedMultiplier = 1.5f;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
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

        character.SetVelocity(jumpDirection * character.JumpSpeed * WallJumpSpeedMultiplier);

        StartCoroutine(WaitForJumpComplete(sm));
    }

    public override void OnUpdate(StateMachine sm)
    {
        character.Move();
    }

    IEnumerator WaitForJumpComplete(StateMachine sm)
    {
        yield return new WaitForSeconds(WallJumpDuration);
        sm.TransitionTo<AirborneSMB>();
    }
}
