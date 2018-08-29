using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterMovement m_characterMovement;
    private FixedUpdateCharacterInput m_input;

    private void Awake()
    {
        m_input = new FixedUpdateCharacterInput();

        m_characterMovement = GetComponentInChildren<CharacterMovement>();
        m_characterMovement.InputController = m_input;
    }

    private void Update()
    {
        m_input.OnUpdate(
            Input.GetAxis("Horizontal"), 
            Input.GetAxis("Vertical"), 
            Input.GetButton("Jump"), 
            Input.GetButton("Fire1"),
            Input.GetButton("Roll"),
            Input.GetButton("CycleWeapon"),
            Input.GetAxis("Left Trigger") == 1);
    }

    private void FixedUpdate()
    {
        // Script Execution order has been modified to ensure that Character should do FixedUpdate last, so it doesn't get
        // reset until all other components have executed their FixedUpdate calls.
        m_input.ResetAfterFixedUpdate();
    }
}
