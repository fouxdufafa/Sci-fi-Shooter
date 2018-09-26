using UnityEngine;
using System.Collections;

public class JellyDamagedState : IState
{
    JellyController jelly;
    Damager damager;
    StateMachine sm;

    public JellyDamagedState(JellyController jelly, Damager damager, StateMachine sm)
    {
        this.jelly = jelly;
        this.damager = damager;
        this.sm = sm;
    }

    public void Enter()
    {
        jelly.SetVelocity(new Vector2(5, 5));
    }

    public void Update()
    {
        sm.ChangeState(new JellyPursuePlayerState(jelly));
    }

    public void Exit()
    {
    }
}
