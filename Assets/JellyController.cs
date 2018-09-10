using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class JellyController : MonoBehaviour {

    public float maxSpeed = 5f;
    public float acceleration = 5f;

    CharacterController2D controller;
    RobotBoyCharacter player;

    StateMachine sm;

    Vector2 velocity = Vector2.zero;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController2D>();
        player = FindObjectOfType<RobotBoyCharacter>();

        sm = new StateMachine();
        sm.ChangeState(new JellyPursuePlayerState(this));
	}

    private void Update()
    {
        sm.Update();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with player");
        }
        if (collision.gameObject.CompareTag("PistolBullet"))
        {
            Debug.Log("Collided with PistolBullet");
            sm.ChangeState(new JellyDamagedState(this, null, sm));
        }
    }

    public void MoveTowardsPlayer()
    {
        Vector2 currentVelocity = controller.velocity;
        Vector2 targetDirection = (player.transform.position - this.transform.position).normalized;
        Vector2 delta = targetDirection * acceleration * Time.deltaTime;
        Vector2 velocity = Vector2.ClampMagnitude(currentVelocity + delta, maxSpeed);
        controller.move(velocity * Time.deltaTime);
    }

    public void SetVelocity(Vector2 velocity)
    {
        controller.velocity = velocity;
    }
}
