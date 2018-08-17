using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePingPongMovement : AbstractMovement {

    [SerializeField] Vector3 destinationOffset;
    [SerializeField] float speed = 1;
    [SerializeField] float destinationCheckRadius = 0.1f;

    Vector3 start;
    Vector3 destination;
    Rigidbody2D rb2d;
    bool movingTowardsDestination;


    private void Start()
    {
        start = transform.position;
        destination = transform.position + destinationOffset;
        rb2d = GetComponent<Rigidbody2D>();
        movingTowardsDestination = true;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (stopped)
        {
            Debug.Log("Stopped movement");
            return;
        }
        if (movingTowardsDestination && IsNearDestination())
        {
            movingTowardsDestination = false;
        }
        else if (!movingTowardsDestination && IsNearStart())
        {
            movingTowardsDestination = true;
        }

        if (movingTowardsDestination)
        {
            rb2d.MovePosition(transform.position + Time.deltaTime * speed * (destination - transform.position).normalized);
        }
        else
        {
            rb2d.MovePosition(transform.position + Time.deltaTime * speed * (start - transform.position).normalized);
        }
	}

    bool IsNearDestination()
    {
        return (destination - transform.position).magnitude <= destinationCheckRadius;
    }

    bool IsNearStart()
    {
        return (start - transform.position).magnitude <= destinationCheckRadius;
    }
}
