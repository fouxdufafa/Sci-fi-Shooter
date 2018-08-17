using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularMovement : AbstractMovement {

    [SerializeField] float radius;
    [SerializeField] float period;
    [SerializeField] float offset; // initial position on the circle in radians between 0 and 2pi

    Rigidbody2D rb2d;
    Vector2 origin;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        origin = rb2d.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float newX = origin.x + (radius * Mathf.Cos(Time.time / period * 2 * Mathf.PI)); // sin(0) = 0, sin(pi) = 0
        float newY = origin.y + (radius * Mathf.Sin(Time.time / period * 2 * Mathf.PI));
        rb2d.transform.position = new Vector2(newX, newY);
	}
}
