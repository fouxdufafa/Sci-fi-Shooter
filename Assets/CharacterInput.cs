using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// What the problem is:
///     In Unity, the only way to get input from the user's keyboard/joysticks/etc. is to poll the Input
///     system in the Update() function. However, there are often times where you need to actually get
///     their input in the FixedUpdate() function (such as when dealing with the physics engine).
///     
/// Why is this a problem?
///     If you use the Input system in FixedUpdate things like GetButton/GetButtonDown will miss key-presses.
///     This is because FixedUpdate runs at 50fps (by default) while the Input system runs at however-fast
///     vsync-is-set. Because of this, you can press and release a key between FixedUpdate calls and 
///     it'll never register if used there.
///     
/// How does this fix it?
///     It gathers input in the Update frames and then stores it until FixedUpdate, after which it will reset
///     the state. It preserves the state of buttons (by only updating if they have been pressed) so even
///     single frame long presses of a key will remain 'true' until FixedUpdate is run, instead of the default
///     behaviour where it turns true and then false again before FixedUpdate is ever run.
/// </summary>
public class FixedUpdateCharacterInput
{
    private class InputState
    {
        public float Horizontal;
        public float Vertical;
        public bool Jump;
        public bool Fire;
        public bool Roll;
        public bool CycleWeapon;
        public bool LeftTrigger;
    }

    public float Horizontal { get { return m_currentState.Horizontal; } }
    public float Vertical { get { return m_currentState.Vertical; } }

    public bool JumpDown { get { return !m_previousState.Jump && m_currentState.Jump; } }

    public bool Jump { get { return m_currentState.Jump; } }

    public bool FireDown { get { return !m_previousState.Fire && m_currentState.Fire; } }

    public bool FireUp { get { return m_previousState.Fire && !m_currentState.Fire;  } }

    public bool Fire { get { return m_currentState.Fire; } }

    public bool RollDown { get { return !m_previousState.Roll && m_currentState.Roll; } }

    public bool Roll { get { return m_currentState.Roll; } }

    public bool WallHugDown { get { return !m_previousState.LeftTrigger && m_currentState.LeftTrigger; } }

    public bool WallHugUp { get { return m_previousState.LeftTrigger && !m_currentState.LeftTrigger; } }

    public bool WallHug { get { return m_currentState.LeftTrigger; } }

    public bool AimDown { get { return !m_previousState.LeftTrigger && m_currentState.LeftTrigger; } }

    public bool AimUp { get { return m_previousState.LeftTrigger && !m_currentState.LeftTrigger; } }

    public bool Aim { get { return m_currentState.LeftTrigger; } }

    public bool CycleWeaponDown { get { return !m_previousState.CycleWeapon && m_currentState.CycleWeapon;  } }

    public bool CycleWeapon { get { return m_currentState.CycleWeapon; } }

    // The current state of the input that is updated on the fly via the Update loop.
    private InputState m_currentState;

    // Store the previous state of the input used on the last FixedUpdate loop,
    // so that we can replicate the difference between GetButton and GetButtonDown.
    private InputState m_previousState;

    // Have we been updated since the last FixedUpdate call? If we haven't been updated,
    // we don't reset. Otherwise, FixedUpdate being called twice in a row will cause
    // JumpDown/FireDown to falsely report being reset.
    private bool m_updatedSinceLastReset;

    public FixedUpdateCharacterInput()
    {
        m_currentState = new InputState();
        m_previousState = new InputState();
    }

    public void OnUpdate(float horizontal, float vertical, bool jump, bool fire, bool roll, bool cycleWeapon, bool leftTrigger)
    {
        // We always take their most up to date horizontal and vertical input. This way we
        // can ignore tiny bursts of accidental press, plus there's some smoothing provided
        // by Unity anyways.
        m_currentState.Horizontal = horizontal;
        m_currentState.Vertical = vertical;

        // However, for button presses we want to catch even single-frame presses between
        // fixed updates. This means that we can only copy across their 'true' status, and not
        // false ones. This means that a single frame press of the button will result in that
        // button reporting 'true' until the end of the next FixedUpdate clearing it. This prevents
        // the loss of very quick button presses which can be very important for jump and fire.
        if (jump)
        {
            m_currentState.Jump = true;
            Debug.Log("Set currentstate jump to true");
        }

        if (fire)
        {
            m_currentState.Fire = true;
        }

        if (roll)
        {
            m_currentState.Roll = true;
        }

        if (cycleWeapon)
        {
            m_currentState.CycleWeapon = true;
        }

        if (leftTrigger)
        {
            m_currentState.LeftTrigger = true;
        }

        m_updatedSinceLastReset = true;
    }

    public void ResetAfterFixedUpdate()
    {
        if (m_currentState.Jump && !m_previousState.Jump)
        {
            Debug.Log("JUMP!!");
        }
        // Don't reset unless we've actually recieved a new set of input from the Update() loop.
        if (!m_updatedSinceLastReset)
            return;

        // Swap the current with the previous and then we'll reset the old
        // previous.
        InputState temp = m_previousState;
        m_previousState = m_currentState;
        m_currentState = temp;

        // We reset the state of single frame events only (that aren't set continuously) as the
        // continious ones will be set from scratch on the next Update() anyways.
        m_currentState.Jump = false;
        m_currentState.Fire = false;
        m_currentState.Roll = false;
        m_currentState.LeftTrigger = false;
        m_updatedSinceLastReset = false;
    }
}
