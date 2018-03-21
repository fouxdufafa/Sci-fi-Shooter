using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHug : MonoBehaviour {

    PlayerInput input;
    PlayerMovement movement;
    WallCheck wallCheck;
    GameObject activeWall;
    
	// Use this for initialization
	void Start () {
        wallCheck = GetComponent<WallCheck>();
        input = GetComponent<PlayerInput>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		// check for wall
	}

    
}
