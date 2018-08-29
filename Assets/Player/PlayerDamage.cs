using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IDamageable {

    public delegate void OnTakeDamage();
    public event OnTakeDamage onTakeDamageObservers;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage (float amount)
    {
        if (onTakeDamageObservers != null)
        {
            onTakeDamageObservers();
        }
    }
}
