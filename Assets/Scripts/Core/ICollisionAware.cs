using UnityEngine;
using System.Collections;

public interface ICollisionAware
{
    void HandleCollision(Collider2D collider);
}
