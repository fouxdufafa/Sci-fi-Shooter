using UnityEngine;
using System.Collections;

public class GroundedSMB : StateMachineBehavior
{
    RobotBoyCharacter character;
    Animator animator;

    public void Start()
    {
        character = GetComponent<RobotBoyCharacter>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnEnter(StateMachine sm)
    {
        character.SetVerticalVelocity(0f);
        animator.SetBool("Ground", true);
    }

    // Update is called once per frame
    public override void OnUpdate(StateMachine sm)
    {
        float velocity = character.MaxHorizontalSpeed * Input.GetAxis("Horizontal");
        character.SetHorizontalVelocity(velocity);
        character.FaceTowardsVelocity();
        character.Move();

        animator.SetFloat("Speed", Mathf.Abs(velocity));

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

        if (Input.GetButtonDown("Roll"))
        {
            sm.TransitionTo<DashingSMB>();
            return;
        }
    }
}
