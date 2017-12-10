using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class EndMenu : MonoBehaviour {

    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    // Use this for initialization
    void Start () {
		Text text = GetComponent<Text>();
		text.text = "Player " + EndData.completionOrder[0].ToString() + " Wins!";
        for (int i = 0; i < 4; ++i)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                playerIndex = testPlayerIndex;
                break;
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        prevState = state;
        state = GamePad.GetState(playerIndex);
        if (Barnout.ButtonPressed(state.Buttons.A, prevState.Buttons.A))
        {
            Barnout.ChangeScene("MainMenu");
        }
        if (Barnout.ButtonPressed(state.Buttons.B, prevState.Buttons.B) || Input.GetKeyDown(KeyCode.Escape))
        {
            Barnout.Quit();
        }
	}
}
