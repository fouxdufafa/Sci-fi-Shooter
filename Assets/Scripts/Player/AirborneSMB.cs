using UnityEngine;
using System.Collections;
using Prime31;

public class AirborneSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    Animator animator;

    public void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
        animator = GetComponent<Animator>();
    }

    public override void OnUpdate(StateMachine sm)
    {
        character.ApplyGravity();
        character.SetHorizontalVelocity(10 * Input.GetAxis("Horizontal"));
        character.FaceTowardsVelocity();
        character.Move();

        if (character.IsGrounded())
        {
            sm.TransitionTo<GroundedSMB>();
        }
    }
}
