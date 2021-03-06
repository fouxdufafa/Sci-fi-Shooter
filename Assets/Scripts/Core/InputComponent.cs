﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class InputComponent : MonoBehaviour
{
    public enum InputType
    {
        MouseAndKeyboard,
        Controller
    }

    public enum XboxControllerButton
    {
        None,
        A,
        B,
        X,
        Y,
        Leftstick,
        Rightstick,
        View,
        Menu,
        LeftBumper,
        RightBumper
    }

    public enum XboxControllerAxis
    {
        None,
        LeftstickHorizontal,
        LeftstickVertical,
        RightstickHorizontal,
        RightstickVertical,
        DpadHorizontal,
        DpadVertical,
        LeftTrigger,
        RightTrigger
    }

    public interface IButtonInput
    {
        bool Down { get; }
        bool Up { get; }
        bool Held { get; }
        void Read();
    }

    public class InputButton : IButtonInput
    {
        public XboxControllerButton controllerButton;
        public bool Down { get; protected set; }
        public bool Up { get; protected set; }
        public bool Held { get; protected set; }

        protected static readonly Dictionary<int, string> buttonsToName = new Dictionary<int, string>
        {
            {(int)XboxControllerButton.A, "A" },
            {(int)XboxControllerButton.B, "B" },
            {(int)XboxControllerButton.X, "X" },
            {(int)XboxControllerButton.Y, "Y" },
        };

        public InputButton(XboxControllerButton controllerButton)
        {
            this.controllerButton = controllerButton;
        }

        public void Read()
        {
            Down = Input.GetButtonDown(buttonsToName[(int)controllerButton]);
            Up = Input.GetButtonUp(buttonsToName[(int)controllerButton]);
            Held = Input.GetButton(buttonsToName[(int)controllerButton]);
        }
    }

    public class InputAxis
    {
        public XboxControllerAxis controllerAxis;
        public float Value;
        float deadZone;

        // For treating "snap" axes as buttons (for instance, left/right triggers)
        public bool Down { get; protected set; }
        public bool Up { get; protected set; }
        public bool Held { get; protected set; }

        float previousValue;

        protected static readonly Dictionary<int, string> axesToName = new Dictionary<int, string>
        {
            {(int)XboxControllerAxis.LeftstickHorizontal, "Leftstick Horizontal" },
            {(int)XboxControllerAxis.LeftstickVertical, "Leftstick Vertical" },
            {(int)XboxControllerAxis.LeftTrigger, "Left Trigger" },
            {(int)XboxControllerAxis.RightTrigger, "Right Trigger" },
        };

        public InputAxis(XboxControllerAxis controllerAxis, float deadZone = 0f)
        {
            this.controllerAxis = controllerAxis;
            this.deadZone = deadZone;
        }

        public void Read()
        {
            Value = Input.GetAxis(axesToName[(int)controllerAxis]);
            if (Mathf.Abs(Value) < deadZone)
            {
                Value = 0f;
            }

            Down = (Value == 1 && previousValue == 0);
            Up = (Value == 0 && previousValue == 1);
            Held = (Value == 1);

            previousValue = Value;
        }
    }

    public class BufferedInputButton : IButtonInput
    {
        InputButton input;
        int frames;
        int frameOfLastRead;

        struct ButtonState
        {
            public int Frame;
            public bool Down;
            public bool Up;
            public bool Held;
        }

        CircularBuffer<ButtonState> buffer;

        public BufferedInputButton(InputButton input, int frameCapacity)
        {
            this.input = input;
            this.buffer = new CircularBuffer<ButtonState>(frameCapacity);
        }

        public void Read()
        {
            int frame = Time.frameCount;
            if (frame != frameOfLastRead)
            {
                frameOfLastRead = frame;
                input.Read();
                ButtonState state;
                state.Down = input.Down;
                state.Up = input.Up;
                state.Held = input.Held;
                state.Frame = frame;
                buffer.Push(state);
            }
        }

        public bool Down { get { return input.Down; } }
        public bool Up { get { return input.Up; } }
        public bool Held { get { return input.Held; } }

        public bool DownBuffered(int frameTolerance = 3)
        {
            return LastNFrames(buffer, frameTolerance).Any((buttonState) => buttonState.Down);
        }

        public bool UpBuffered(int frameTolerance = 3)
        {
            return LastNFrames(buffer, frameTolerance).Any((buttonState) => buttonState.Up);
        }

        public bool HeldBuffered(int frameTolerance = 3)
        {
            return LastNFrames(buffer, frameTolerance).Any((buttonState) => buttonState.Held);
        }

        private IEnumerable<ButtonState> LastNFrames(CircularBuffer<ButtonState> buffer, int frames)
        {
            return buffer.Skip(Mathf.Max(0, buffer.Count - frames));
        }
    }

    abstract protected void GetInputs();

    private void Update()
    {
        GetInputs();
    }
}
