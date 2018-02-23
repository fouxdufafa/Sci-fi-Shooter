using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D character;
    private bool jump;


    private void Awake()
    {
        character = GetComponent<PlatformerCharacter2D>();
    }


    private void Update()
    {
        if (!jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        bool wallHugPressed = Input.GetAxis("Wall Hug") > 0;

        // Pass all parameters to the character control script.
        character.Move(h, crouch, jump, wallHugPressed);
        jump = false;
    }
}
