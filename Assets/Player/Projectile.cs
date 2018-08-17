using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float destroyAfterSeconds;

    private void Start()
    {
        Destroy(gameObject, 1);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
