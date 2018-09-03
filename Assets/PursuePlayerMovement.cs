using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuePlayerMovement : AbstractMovement {

    // Use this for initialization
    [SerializeField] GameObject target;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] bool rotate = false;
    [SerializeField] float angularAcceleration = 10f;
    [SerializeField] float knockbackDuration = 0.2f;

    Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (stopped) {
            rb2d.velocity = Vector2.zero; // TODO: make this a smooth stop
            return;
        }
        Vector2 toTarget = (target.transform.position - transform.position).normalized;
        rb2d.velocity += Time.deltaTime * acceleration * toTarget;

        if (rb2d.velocity.magnitude > maxSpeed)
        {
            rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
        }

        if (rotate)
        {
            Vector2 fromTo = target.transform.position - transform.position;
            transform.rotation = Quaternion.FromToRotation(Vector2.up, fromTo);
        }
    }
}
