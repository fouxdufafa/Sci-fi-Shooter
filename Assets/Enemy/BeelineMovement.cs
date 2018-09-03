using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeelineMovement : AbstractMovement {

    // Use this for initialization
    [SerializeField] GameObject target;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] bool faceTarget = false;
    [SerializeField] float knockbackDuration = 0.1f;
    [SerializeField] float knockbackSpeed = 20f;
    [SerializeField] bool restoreOldVelocityAfterKnockback = false;
    [SerializeField] AudioClip knockbackSound;

    Rigidbody2D rb2d;
    Coroutine knockbackRoutine;
    Vector2 oldVelocity;
    bool isInKnockback = false;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stopped)
        {
            rb2d.velocity = Vector2.zero; // TODO: make this a smooth stop
            return;
        }
        if (isInKnockback)
        {
            return;
        }

        MoveTowardsTarget();

        if (faceTarget)
        {
            FaceTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector2 toTarget = (target.transform.position - transform.position).normalized;
        Vector2 delta = acceleration * toTarget;

        AddVelocity(delta);

        ClampVelocity(maxSpeed);
    }

    void AddVelocity(Vector2 velocity)
    {
        rb2d.velocity += velocity;
    }

    void SetVelocity(Vector2 velocity)
    {
        rb2d.velocity = velocity;
    }

    void ClampVelocity(float maxSpeed)
    {
        if (rb2d.velocity.magnitude > maxSpeed)
        {
            rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
        }
    }

    void FaceTarget()
    {
        Vector2 fromTo = target.transform.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector2.up, fromTo);
    }

    public override void Knockback(Vector2 velocity, bool stopMomentum)
    {
        if (knockbackRoutine != null)
        {
            StopCoroutine(knockbackRoutine);
        }
        else
        {
            // Save the current velocity before we knockback, so we can restore it afterwards
            // This gives a nice "bounce-back" effect
            oldVelocity = rb2d.velocity;
        }
        knockbackRoutine = StartCoroutine(KnockbackRoutine(velocity, stopMomentum));
    }

    IEnumerator KnockbackRoutine(Vector2 velocity, bool stopMomentum)
    {
        isInKnockback = true;
        if (stopMomentum)
        {
            SetVelocity(Vector2.zero);
        }
        AddVelocity(velocity.normalized * knockbackSpeed);
        yield return new WaitForSeconds(knockbackDuration);
        isInKnockback = false;
        if (restoreOldVelocityAfterKnockback)
        {
            SetVelocity(oldVelocity);
        }
    }
}
