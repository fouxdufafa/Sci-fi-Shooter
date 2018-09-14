using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniJelly : MonoBehaviour {

    Health health;
    Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        health.onDieObservers += OnDie;
    }

    void OnDie ()
    {
        Destroy(gameObject);
    }
}
