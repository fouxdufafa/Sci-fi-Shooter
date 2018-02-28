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
    PlayerMovement character;
    float horizontal;
    float vertical;
    bool jump;
    TriggerState leftTriggerState;

    // walking running stopped jumping wallhugging
    // groundaim wallaim

    void Awake()
    {
        character = GetComponent<PlayerMovement>();
        leftTriggerState = TriggerState.No;
    }


    void Update()
    {
        if (!jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        vertical = CrossPlatformInputManager.GetAxis("Vertical");

        ReadWallHugInput();
    }

    void FixedUpdate()
    {
        // Pass all parameters to the character control script.
        character.Move(horizontal, vertical, jump, leftTriggerState);
        jump = false;
    }

    void ReadWallHugInput()
    {
        bool wallHugPressed = Input.GetAxis("Left Trigger") == 1;

        if (wallHugPressed)
        {
            if (leftTriggerState == TriggerState.No || leftTriggerState == TriggerState.End)
            {
                leftTriggerState = TriggerState.Start;
            }
            else
            {
                leftTriggerState = TriggerState.Stay;
            }
        }
        else
        {
            if (leftTriggerState == TriggerState.Stay)
            {
                leftTriggerState = TriggerState.End;
            }
            else
            {
                leftTriggerState = TriggerState.No;
            }
        }
    }
}
