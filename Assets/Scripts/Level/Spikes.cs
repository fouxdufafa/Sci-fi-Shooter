using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    BoxCollider2D spikeCollider;
    BoxCollider2D baseCollider;

	// Use this for initialization
	void Start () {
        spikeCollider = transform.Find("SpikeCollider").GetComponent<BoxCollider2D>();
        baseCollider = transform.Find("BaseCollider").GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	public float BaseWidth { get { return baseCollider.size.x; } }
}
