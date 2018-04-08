using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class MainMenuHandler : MonoBehaviour {

    public MM_AudioMaster audioMaster;

    public MenuButton[] buttons;
    private int currButton = 0;

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    bool axisBuffer = true;


    private void Start()
    {
        audioMaster.PlayMusic();
        buttons[currButton].ToHover();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                    break;
                }
            }
        }


        prevState = state;
        state = GamePad.GetState(playerIndex);

        if (state.ThumbSticks.Left.Y == 0)
        {
            axisBuffer = true;
        }

        Cursor.lockState = CursorLockMode.None;
        if (state.ThumbSticks.Left.Y < 0 && axisBuffer)
        {
            buttons[currButton].ToUnselected();
            currButton++;
            if (currButton >= buttons.Length) currButton = 0;
            buttons[currButton].ToHover();
            axisBuffer = false;
            audioMaster.PlayToggle();
        }
        else if (state.ThumbSticks.Left.Y > 0 && axisBuffer)
        {
            buttons[currButton].ToUnselected();
            currButton--;
            if (currButton < 0) currButton = buttons.Length - 1;
            buttons[currButton].ToHover();
            axisBuffer = false;
            audioMaster.PlayToggle();
        }
        if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
        {
            buttons[currButton].ActivateButton();
            audioMaster.PlaySelect();
        }
        
    }

}
