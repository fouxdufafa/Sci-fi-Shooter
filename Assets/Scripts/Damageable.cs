using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable {

    public delegate void OnTakeDamage(int amount);
    public event OnTakeDamage onTakeDamageListeners;

    public void TakeDamage (int amount)
    {
        if (onTakeDamageListeners != null)
        {
            onTakeDamageListeners(amount);
        }
    }
}
