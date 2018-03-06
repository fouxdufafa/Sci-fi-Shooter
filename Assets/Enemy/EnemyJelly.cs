using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJelly : MonoBehaviour {

    [SerializeField] GameObject target;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackImpulseForce = 10f;
    [SerializeField] float attackStunSeconds = .5f;

    private Vector3 lastContactNormal;

    Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        // nudge jelly in the direction of player
        Vector2 toTarget = (target.transform.position - transform.position).normalized;
        rb2d.velocity += Time.deltaTime * acceleration * toTarget;

        if (rb2d.velocity.magnitude > maxSpeed)
        {
            rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.transform.position, 1.1f);

        if (lastContactNormal != null)
        {
            Gizmos.DrawLine(transform.position, transform.position + (lastContactNormal * 5));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[10];
            print(collision.GetContacts(contacts));
            print("Got contact point");
            Vector2 normal = contacts[0].normal * -1;
            lastContactNormal = normal;
            Vector2 impulseDirection = normal.x >= 0 ? Vector2.right : Vector2.left;
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(attackDamage, attackStunSeconds);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(impulseDirection* attackImpulseForce, ForceMode2D.Impulse);
        }
    }
}
