using UnityEngine;
using System.Collections;
using System.Linq;

public class StateMachine
{
    protected IState currentState;
    protected IState previousState;

    public void Update()
    {
        currentState.Update();
    }

    public void ChangeState(IState state)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        previousState = currentState;
        currentState = state;
        currentState.Enter();
    }
}
