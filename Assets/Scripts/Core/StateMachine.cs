using UnityEngine;
using System.Collections;
using System.Linq;

public class StateMachine
{
    protected IState currentState;

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
        currentState = state;
        currentState.Enter();
    }
}
