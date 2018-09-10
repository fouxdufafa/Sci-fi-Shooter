using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JellyPursuePlayerState : IState
{
    JellyController jelly;
    List<Collider2D> collisions;

    // Use this for initialization
    public JellyPursuePlayerState(JellyController jelly)
    {
        this.jelly = jelly;
        this.collisions = new List<Collider2D>();
    }

    // Update is called once per frame
    public void Enter()
    {
    }

    public void Update()
    {
        ProcessCollisions();
        jelly.MoveTowardsPlayer();
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {

    }

    void ProcessCollisions()
    {
        foreach (Collider2D collision in collisions)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Collided with player");
                jelly.SetVelocity(new Vector2(5, 5));
            } 
            if (collision.gameObject.CompareTag("PistolBullet"))
            {
                Debug.Log("Collided with PistolBullet");
                jelly.SetVelocity(new Vector2(5, 5));
            }
        }
        collisions.Clear();
    }
}
