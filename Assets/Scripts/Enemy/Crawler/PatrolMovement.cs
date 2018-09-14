using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour {

    [SerializeField] float speed;

    Collider2D c2d;
    RaycastHit2D[] hits;

    // Use this for initialization
    void Start () {
        c2d = GetComponent<Collider2D>();
        hits = new RaycastHit2D[5];
    }
	
	// Update is called once per frame
	public void Move () {
        if (c2d.Cast(transform.right, hits, speed * Time.deltaTime) != 0)
        {
            Flip();
        }
        transform.Translate(transform.right * Mathf.Sign(transform.localScale.x) * speed * Time.deltaTime, Space.World);
	}

    void Flip ()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 10);
    }
}
