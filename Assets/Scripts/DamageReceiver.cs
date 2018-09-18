using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageReceiver : MonoBehaviour, IDamageable {

    IDamageable damageable;

	// Use this for initialization
	void Start () {
        damageable = transform.root.GetComponent<IDamageable>();
	}

    // Update is called once per frame
    public void TakeDamage(Damager damager)
    {
        if (damageable != null && isActiveAndEnabled)
        {
            damageable.TakeDamage(damager);
        }
    }
}
