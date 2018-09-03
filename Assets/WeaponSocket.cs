using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket : MonoBehaviour {

    // Use this for initialization
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
