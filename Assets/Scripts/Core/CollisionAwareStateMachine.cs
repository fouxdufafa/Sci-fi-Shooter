using UnityEngine;
using System.Collections;

public class CollisionAwareStateMachine : StateMachine, ICollisionAware
{
    public void HandleCollision(Collider2D collider)
    {
        if (currentState is ICollisionAware)
        {
            (currentState as ICollisionAware).HandleCollision(collider);
        }
    }
}
