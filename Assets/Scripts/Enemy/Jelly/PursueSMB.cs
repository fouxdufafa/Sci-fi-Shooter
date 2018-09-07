using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PursueSMB : StateMachineBehavior
{
    JellyController jelly;
    List<Collider2D> collisions = new List<Collider2D>();

    // Use this for initialization
    void Start()
    {
        jelly = GetComponent<JellyController>();
    }

    // Update is called once per frame
    public override void OnEnter(StateMachine sm)
    {
        base.OnEnter(sm);
    }

    public override void OnUpdate(StateMachine sm)
    {
        ProcessCollisions(sm);
        jelly.MoveTowardsPlayer();
    }

    void ProcessCollisions(StateMachine sm)
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        collisions.Add(collision);
    }
}
