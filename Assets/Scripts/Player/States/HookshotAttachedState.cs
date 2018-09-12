using UnityEngine;
using UnityEditor;

public class HookshotAttachedState : IState, ICollisionAware
{
    RobotBoyCharacter character;
    StateMachine sm;
    Hookshot hookshot;

    float epsilon = 1f;

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
    }

    public void Update()
    {
        character.Move();
    }

    public void Exit()
    {
        hookshot.Remove();
        character.SetVelocity(Vector2.zero);
    }

    public void HandleCollision(Collider2D collider)
    {
        Debug.Log("Hookshot interrupted by collision");
        sm.ChangeState(new AirborneState(character));
    }
}