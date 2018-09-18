using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour {

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            DamageReceiver receiver = collision.gameObject.GetComponent<DamageReceiver>();
            receiver.TakeDamage(new Damager(10f, DamageForce.Average, DamageType.OneShot, false, gameObject));
        }
    }
}
