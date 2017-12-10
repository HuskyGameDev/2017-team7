using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Inputs : MonoBehaviour
{

    private static Controller[] controllers;
    private int numPlayers = 4;

    

    // Use this for initialization
    void Awake()
    {
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

                controllers[player].UpdateValues(lsX, lsClick, rsClick, lt, rt, lb, rb,
                                    aButton, bButton, xButton, yButton, back, start);
            }
            
        }
    }

    // Returns Controller object for the given playerNum
    public static Controller GetController(int playerNum)
    {
        return controllers[playerNum - 1];
    }
}
