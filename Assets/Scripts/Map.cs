using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Map : MonoBehaviour {

	public AudioSource Snd_Beep;
	public AudioSource Snd_Go;
	public AudioSource Msc_Banjo;

	PlayerIndex pausePlayerIndex;
    GamePadState pausePlayerState;
    GamePadState pausePlayerPrevState;
	bool paused = false;
	bool countdown = true;
	float time;
	int count = 4;

	GamePadState[] testStates = new GamePadState[4];

	// Use this for initialization
	void Start () {
		time = Time.unscaledTime;
		Debug.Log(time);
	}
	
	// Update is called once per frame
	void Update () {

		if (!countdown) {
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

		

		
	}

	void FixedUpdate() {
		if (countdown) {
		if (Time.unscaledTime - time >= 1)
		{
			time = Time.unscaledTime;
			count--;
			if (count == 0) {
				
			Debug.Log("Go!");
				Snd_Go.Play();
				Msc_Banjo.PlayDelayed(0.3F);
			countdown = false;
			}
			else {
				Snd_Beep.Play();
			Debug.Log(count);
			}
		}
		}
	}

	public bool isPaused() {
		return paused;
	}

	public bool inCountdown() {
		return countdown;
	}

    private void OnTriggerEnter2D(Collider2D collision) {

    }
}
