using UnityEngine;
using System.Collections;

public class GroundedSMB : StateMachineBehavior
{
    RobotBoyCharacter character;

    public void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
    }

    public override void OnEnter(StateMachine sm)
    {
        character.SetVerticalVelocity(0f);
    }

    // Update is called once per frame
    public override void OnUpdate(StateMachine sm)
    {
        character.SetHorizontalVelocity(10 * Input.GetAxis("Horizontal"));
        character.FaceTowardsVelocity();
        character.Move();

        if (!character.IsGrounded())
        {
            sm.TransitionTo<AirborneSMB>();
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            character.SetVerticalVelocity(20f);
            sm.TransitionTo<AirborneSMB>();
            return;
        }
    }
}
