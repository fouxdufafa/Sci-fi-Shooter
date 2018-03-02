using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// TODO: use event delegates instead
public enum TriggerState
{
    No = 0,
    Start = 1,
    Stay = 2,
    End = 3
}

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    PlayerMovement movement;
    float horizontalMove;
    float verticalMove;
    float horizontalAim;
    float verticalAim;
    bool jump;
    bool fire;
    TriggerState leftTriggerState;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        leftTriggerState = TriggerState.No;
    }

    void Update()
    {
        horizontalMove = CrossPlatformInputManager.GetAxis("Horizontal");
        verticalMove = CrossPlatformInputManager.GetAxis("Vertical");
        horizontalAim = CrossPlatformInputManager.GetAxis("Horizontal Aim");
        verticalAim = CrossPlatformInputManager.GetAxis("Vertical Aim");

        if (!jump)
        {
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!fire)
        {
            fire = CrossPlatformInputManager.GetButtonDown("Fire1");
        }

        leftTriggerState = ReadLeftTrigger(leftTriggerState);
    }

    void FixedUpdate()
    {
        // Pass all parameters to the character control script.
        movement.Move(horizontalMove, verticalMove, jump, leftTriggerState, horizontalAim, verticalAim, fire);
        jump = false;
        fire = false;
    }

    TriggerState ReadLeftTrigger(TriggerState previous)
    {
        bool triggerPressed = Input.GetAxis("Left Trigger") == 1;
        TriggerState next;
        if (triggerPressed)
        {
            if (previous == TriggerState.No || previous == TriggerState.End)
            {
                next = TriggerState.Start;
            }
            else
            {
                next = TriggerState.Stay;
            }
        }
        else
        {
            if (previous == TriggerState.Stay)
            {
                next = TriggerState.End;
            }
            else
            {
                next = TriggerState.No;
            }
        }

        return next;
    }
}
