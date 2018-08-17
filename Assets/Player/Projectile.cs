using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] int damage = 10;
    [SerializeField] float destroyAfterSeconds;

    private void Start()
    {
        Destroy(gameObject, 1);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (!collider.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log("Hit an IDamageable");
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
