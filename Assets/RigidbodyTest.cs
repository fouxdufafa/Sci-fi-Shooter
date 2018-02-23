using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyTest : MonoBehaviour {

    Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(0, -1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
