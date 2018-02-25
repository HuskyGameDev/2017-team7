using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Inputs : MonoBehaviour
{

    private static Controller[] controllers;
    private int numPlayers = 4;
    private static bool alreadyInitialized = false;
    

    // Use this for initialization
    void Awake()
    {
        if(alreadyInitialized){
            Debug.LogError("Multiple instance of Inputs detected! Please remove an instance of Inputs.");
            return;
        }
        alreadyInitialized = true;
        controllers = new Controller[numPlayers];

        for (int i = 0; i < numPlayers; i++)
        {
            controllers[i] = new Controller();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int player = 0; player < numPlayers; player++)
        {
            PlayerIndex playerIndex = (PlayerIndex)player;
            GamePadState state = GamePad.GetState(playerIndex);

            if (state.IsConnected)
            {
                float lsX = state.ThumbSticks.Left.X;
                float lsY = state.ThumbSticks.Left.Y;
                float rsX = state.ThumbSticks.Right.X;
                float rsY = state.ThumbSticks.Right.Y;
                bool lsClick = state.Buttons.LeftStick == ButtonState.Pressed;
                bool rsClick = state.Buttons.RightStick == ButtonState.Pressed;

                float rt = state.Triggers.Right;
                float lt = state.Triggers.Left;
                bool lb = state.Buttons.LeftShoulder == ButtonState.Pressed;
                bool rb = state.Buttons.RightShoulder == ButtonState.Pressed;

                bool aButton = state.Buttons.A == ButtonState.Pressed;
                bool bButton = state.Buttons.B == ButtonState.Pressed;
                bool xButton = state.Buttons.X == ButtonState.Pressed;
                bool yButton = state.Buttons.Y == ButtonState.Pressed;
                bool back = state.Buttons.Back == ButtonState.Pressed;
                bool start = state.Buttons.Start == ButtonState.Pressed;

                bool dpadUp = state.DPad.Up == ButtonState.Pressed;
                bool dpadDown = state.DPad.Down == ButtonState.Pressed;
                bool dpadLeft = state.DPad.Left == ButtonState.Pressed;
                bool dpadRight = state.DPad.Right == ButtonState.Pressed;

                controllers[player].UpdateValues(lsX, lsY, lsClick, rsX, rsY, rsClick, lt, rt, lb, rb,
                                    aButton, bButton, xButton, yButton, back, start, dpadUp, dpadDown, dpadLeft, dpadRight);
            }
            
        }
    }

    // Returns Controller object for the given playerNum
    public static Controller GetController(int playerNum)
    {
        return controllers[playerNum - 1];
    }
}
