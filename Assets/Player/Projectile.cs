using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] int damage = 10;
    [SerializeField] float destroyAfterSeconds;

    private void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject);
        IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Debug.Log("Hit an IDamageable");
            damageable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log(collider.gameObject);
        IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Debug.Log("Hit an IDamageable");
            damageable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
