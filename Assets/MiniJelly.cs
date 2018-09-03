using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniJelly : MonoBehaviour {

    Damageable health;
    Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        health = GetComponent<Damageable>();
        health.onDieObservers += OnDie;
    }

    void OnDie ()
    {
        Destroy(gameObject);
    }
}
