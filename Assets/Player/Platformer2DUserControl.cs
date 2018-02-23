using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// TODO: use event delegates instead
public enum WallHugButtonState
{
    No = 0,
    Start = 1,
    Remain = 2,
    End = 3
}

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D character;
    private float horizontal;
    private bool jump;
    private WallHugButtonState wallHugState;

    private void Awake()
    {
        character = GetComponent<PlatformerCharacter2D>();
        wallHugState = WallHugButtonState.No;
    }


    private void Update()
    {
        if (!jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        
        ReadWallHugInput();
    }

    private void FixedUpdate()
    {
        // Pass all parameters to the character control script.
        character.Move(horizontal, jump, wallHugState);
        jump = false;
    }

    void ReadWallHugInput()
    {
        bool wallHugPressed = Input.GetAxis("Wall Hug") == 1;

        if (wallHugPressed)
        {
            if (wallHugState == WallHugButtonState.No || wallHugState == WallHugButtonState.End)
            {
                wallHugState = WallHugButtonState.Start;
            }
            else
            {
                wallHugState = WallHugButtonState.Remain;
            }
        }
        else
        {
            if (wallHugState == WallHugButtonState.Remain)
            {
                wallHugState = WallHugButtonState.End;
            }
            else
            {
                wallHugState = WallHugButtonState.No;
            }
        }
    }
}
