using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMovement : MonoBehaviour {

    protected bool stopped = false;

    public void Stop()
    {
        stopped = true;
    }

    public void Resume()
    {
        stopped = false;
    }

    public virtual void Knockback(Vector2 velocity, bool stopMomentum) { }
}
