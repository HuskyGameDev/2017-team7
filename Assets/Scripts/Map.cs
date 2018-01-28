using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Map : MonoBehaviour {

	PlayerIndex pausePlayerIndex;
    GamePadState pausePlayerState;
    GamePadState pausePlayerPrevState;
	bool paused = false;

	GamePadState[] testStates = new GamePadState[4];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (!paused) {
			for (int i = 0; i < 4; i++) {

				PlayerIndex testPlayerIndex = (PlayerIndex) i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);

				if (testState.IsConnected) {
					
					if (Barnout.ButtonPressed(testState.Buttons.Start, 
										testStates[i].Buttons.Start)) {
						pausePlayerIndex = testPlayerIndex;
						pausePlayerState = testState;
						if (Time.timeScale == 1.0F) {
							Time.timeScale = 0.0F;
							paused = true;
							Debug.Log("PAUSED");
						}
					}
				}
				testStates[i] = testState;
			}
		}
		else {
			pausePlayerPrevState = pausePlayerState;
			pausePlayerState = GamePad.GetState(pausePlayerIndex);
			if (Barnout.ButtonPressed(pausePlayerState.Buttons.Start, 
										pausePlayerPrevState.Buttons.Start)) {
				Time.timeScale = 1.0F;
				paused = false;
				Debug.Log("NOT PAUSED");
			}
								
		}

		
	}

    private void OnTriggerEnter2D(Collider2D collision) {

    }
}
