using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JellyPursuePlayerState : IState
{
    JellyController jelly;

    // Use this for initialization
    public JellyPursuePlayerState(JellyController jelly)
    {
        this.jelly = jelly;
    }

    // Update is called once per frame
    public void Enter()
    {
    }

    public void Update()
    {
        jelly.MoveTowardsPlayer();
    }

    public void Exit()
    {
    }
}
