using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class CrawlerController : MonoBehaviour, IDamageable {

    public CharacterController2D controller { get; private set; }
    public CollisionAwareStateMachine sm { get; private set; }
    public Animator animator { get; private set; }
    public SpriteFlasher flasher { get; private set; }

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        flasher = GetComponent<SpriteFlasher>();
        sm = new CollisionAwareStateMachine();
        sm.ChangeState(new Idle(this));
	}
	
	// Update is called once per frame
	void Update () {
        sm.Update();
	}

    public void TakeDamage(Damager damager)
    {
        flasher.Flash();
    }
}
