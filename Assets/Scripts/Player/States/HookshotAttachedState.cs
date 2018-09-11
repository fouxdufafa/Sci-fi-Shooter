using UnityEngine;
using UnityEditor;

public class HookshotAttachedState : IState
{
    RobotBoyCharacter character;
    StateMachine sm;
    Hookshot hookshot;

    float epsilon = 1f;

    public HookshotAttachedState(RobotBoyCharacter character, StateMachine sm, Hookshot hookshot)
    {
        this.character = character;
        this.sm = sm;
        this.hookshot = hookshot;
    }

    public void Enter()
    {
        Vector2 velocity = (hookshot.transform.position - character.transform.position).normalized * hookshot.Speed;
        character.SetVelocity(velocity);
    }

    public void Update()
    {
        character.Move();
        Vector2 distance = hookshot.transform.position - character.transform.position;
        if (distance.magnitude < epsilon)
        {
            Debug.Log("Detaching from hookshot");
            character.DetachFromHookshot(hookshot);
        }
    }

    public void Exit()
    {
        hookshot.Remove();
        character.SetVelocity(Vector2.zero);
    }
}