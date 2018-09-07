using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyDamage : MonoBehaviour {

    JellyController movement;

    private void Start()
    {
        movement = GetComponent<JellyController>();
    }
}
