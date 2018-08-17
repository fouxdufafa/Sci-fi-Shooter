using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractMovement : MonoBehaviour {

    protected bool stopped = false;

    public void Stop()
    {
        stopped = true;
    }

    public void Resume()
    {
        stopped = false;
    }
}
