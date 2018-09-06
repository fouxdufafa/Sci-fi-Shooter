using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : InputComponent
{
    public BufferedInputButton Jump = new BufferedInputButton(new InputButton(XboxControllerButton.A), 20);
    public InputButton Dash = new InputButton(XboxControllerButton.B);
    public InputButton Fire = new InputButton(XboxControllerButton.X);
    public InputButton CycleWeapon = new InputButton(XboxControllerButton.Y);
    public InputAxis HorizontalMovement = new InputAxis(XboxControllerAxis.LeftstickHorizontal);
    public InputAxis VerticalMovement = new InputAxis(XboxControllerAxis.LeftstickVertical);
    public InputAxis WallHug = new InputAxis(XboxControllerAxis.LeftTrigger);

    // Use this for initialization
    protected override void GetInputs()
    {
        Jump.Read();
        Dash.Read();
        Fire.Read();
        CycleWeapon.Read();
        HorizontalMovement.Read();
        VerticalMovement.Read();
        WallHug.Read();
    }
}
