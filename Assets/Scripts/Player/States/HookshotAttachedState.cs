using UnityEngine;
using UnityEditor;

public class HookshotAttachedState : IState, ICollisionAware
{
    RobotBoyCharacter character;
    StateMachine sm;
    Hookshot hookshot;

    float epsilon = 1f;
    float previousDistance;

    public HookshotAttachedState(RobotBoyCharacter character)
    {
        this.character = character;
        this.sm = character.sm;
        this.hookshot = character.hookshotInstance;
    }

    public void Enter()
    {
        Vector2 velocity = (hookshot.transform.position - character.transform.position).normalized * hookshot.ReelInSpeed;
        character.SetVelocity(velocity);
        character.ReleaseWeapon();

        previousDistance = Vector2.Distance(character.transform.position, hookshot.transform.position);
    }

    public void Update()
    {
        character.Move();

        float distanceToHookshot = Vector2.Distance(character.transform.position, hookshot.transform.position);
        if (distanceToHookshot > previousDistance)
        {
            character.DetachFromHookshot(hookshot);
        }
        previousDistance = distanceToHookshot;
    }

    public void Exit()
    {
        hookshot.Remove();
        character.SetVelocity(Vector2.zero);
    }

    public void HandleCollision(Collider2D collider)
    {
        Debug.Log("Hookshot interrupted by collision");
        character.DetachFromHookshot(hookshot);
    }
}