﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class JellyController : MonoBehaviour {

    public float maxSpeed = 5f;
    public float acceleration = 5f;

    CharacterController2D controller;
    RobotBoyCharacter player;

    Vector2 velocity = Vector2.zero;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController2D>();
        player = FindObjectOfType<RobotBoyCharacter>();
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