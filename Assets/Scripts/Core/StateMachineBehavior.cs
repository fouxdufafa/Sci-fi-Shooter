using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBehavior : MonoBehaviour {

    public bool IsStartBehavior = false;

    public virtual void OnEnter(StateMachine sm)
    {
        
    }
	
    public virtual void OnUpdate(StateMachine sm)
    {

    }

    public virtual void OnExit(StateMachine sm)
    {

    }
}
