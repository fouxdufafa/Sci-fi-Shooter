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
    float horizontalMove;
    float verticalMove;
    float horizontalAim;
    float verticalAim;
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

        horizontalMove = CrossPlatformInputManager.GetAxis("Horizontal");
        verticalMove = CrossPlatformInputManager.GetAxis("Vertical");
        horizontalAim = CrossPlatformInputManager.GetAxis("Horizontal Aim");
        verticalAim = CrossPlatformInputManager.GetAxis("Vertical Aim");

        ReadWallHugInput();
    }

    void FixedUpdate()
    {
        // Pass all parameters to the character control script.
        character.Move(horizontalMove, verticalMove, jump, leftTriggerState, horizontalAim, verticalAim);
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
