using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJelly : MonoBehaviour {

    [SerializeField] GameObject deathPrefab;

    // Movement and animation
    AbstractMovement movement;
    Animator animator;
    BoltAttackScript attackRoutine;
    Health health;
    AudioSource audioSource;
    int zapPrepare;
    int zapStart;
    int zapEnd;
    
	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        health.onDieObservers += OnDie;
        movement = GetComponent<AbstractMovement>();
        animator = GetComponent<Animator>();
        attackRoutine = GetComponent<BoltAttackScript>();
        attackRoutine.onAimObservers += OnAim;
        attackRoutine.onFireObservers += OnFire;
        attackRoutine.onCeaseFireObservers += OnCeaseFire;
        zapPrepare = Animator.StringToHash("ZapPrepare");
        zapStart = Animator.StringToHash("ZapStart");
        zapEnd = Animator.StringToHash("ZapEnd");
	}

    void OnAim()
    {
        animator.SetTrigger(zapPrepare);
        movement.Stop();
    }

    void OnFire()
    {
        animator.SetTrigger(zapStart);
    }

    void OnCeaseFire()
    {
        animator.SetTrigger(zapEnd);
        movement.Resume();
    }

    void OnDie()
    {
        // Create death ball
        Instantiate(deathPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
