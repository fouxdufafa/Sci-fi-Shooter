using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] int damage = 10;
    [SerializeField] float destroyAfterSeconds;

    Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(gameObject, destroyAfterSeconds);
    }

    // TODO: Lookup types of applicable collisions
    void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider);
    }

    // TODO: lookup types of applicable collisions
    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    void HandleCollision(Collider2D collider)
    {
        Damageable damageable = collider.gameObject.GetComponent<Damageable>();
        AbstractMovement movement = collider.gameObject.GetComponent<AbstractMovement>();
        if (damageable != null)
        {
            Debug.Log("Hit an IDamageable");
            damageable.TakeDamage(damage);
        }
        if (movement != null)
        {
            KnockbackWithBulletVelocity(collider, movement);
        }
        Destroy(gameObject);
    }

    void KnockbackWithBulletVelocity(Collider2D collider, AbstractMovement movement)
    {
        Vector2 dir = rb2d.velocity;
        movement.Knockback(dir, true);
    }

    void KnockbackWithContactNormal(Collider2D collider, AbstractMovement movement)
    {
        ContactPoint2D[] contactPoints = new ContactPoint2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.maxNormalAngle = 20;
        if (collider.GetContacts(filter, contactPoints) > 0)
        {
            ContactPoint2D thePoint = contactPoints[0];
            Vector2 dir = thePoint.normal;
            movement.Knockback(dir, true);
        }
    }
}
