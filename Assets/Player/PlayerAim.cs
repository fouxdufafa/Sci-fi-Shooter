using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour {

    public delegate void OnAimChange(Vector2 direction);
    public event OnAimChange onAimChangeListeners;

    public void Aim(Vector2 direction)
    {

    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
