using UnityEngine;
using System.Collections;
using System.Linq;

public class StateMachine: MonoBehaviour
{
    [HideInInspector]
    public StateMachineBehavior currentState;

    // Use this for initialization
    void Start()
    {
        currentState = FindInitialState();
        currentState.OnEnter(this);
    }

    StateMachineBehavior FindInitialState()
    {
        return GetComponents<StateMachineBehavior>().First(smb => smb.IsStartBehavior);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate(this);
    }

    public void TransitionTo<T>() where T : StateMachineBehavior
    {
        StateMachineBehavior next = GetComponent<T>();
        currentState.OnExit(this);
        currentState = next;
        currentState.OnEnter(this);
    }
}
