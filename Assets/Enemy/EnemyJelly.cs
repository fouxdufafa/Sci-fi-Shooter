using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJelly : MonoBehaviour {

    // Movement and animation
    AbstractMovement movement;
    Animator animator;
    BoltAttackScript attackRoutine;
    int zapPrepare;
    int zapStart;
    int zapEnd;
    
	// Use this for initialization
	void Start () {
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
}
