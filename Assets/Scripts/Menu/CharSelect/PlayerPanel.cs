using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;


public class PlayerPanel : MonoBehaviour {

    private enum STATES { DISCONNECTED, UNJOINED, SELECTING, READY };
    private STATES state;

	public int playerNum;

    private Animator animator;

    PlayerIndex playerIndex;
    GamePadState gp_state;
    GamePadState prev_gp_state;


	void Start () {
        animator = GetComponent<Animator>();
        state = STATES.DISCONNECTED;
        playerIndex = (PlayerIndex)(playerNum - 1);
        Debug.Log("" + playerIndex + " " + playerNum);
    }
	





	void Update () {

        prev_gp_state = gp_state;
        gp_state = GamePad.GetState(playerIndex);

        if (!gp_state.IsConnected && state != STATES.DISCONNECTED)
        {
            state = STATES.DISCONNECTED;
            animator.SetTrigger("Disconnected");
        }

        switch (state) {
            case STATES.DISCONNECTED:
                if(gp_state.IsConnected)
                {
                    animator.SetTrigger("Connected");
                    state = STATES.UNJOINED;
                }


                break;
            case STATES.UNJOINED:

                if (Barnout.ButtonPressed(gp_state.Buttons.Start, prev_gp_state.Buttons.Start))
                {
                   animator.SetTrigger("Joined");
                   state = STATES.SELECTING;
                }
                else if (Barnout.ButtonPressed(gp_state.Buttons.B, prev_gp_state.Buttons.B))
                {
                    Barnout.ChangeScene("MainMenu");
                }
                break;
            case STATES.SELECTING:
                if (Barnout.ButtonPressed(gp_state.Buttons.B, prev_gp_state.Buttons.B))
                {
                    animator.SetTrigger("UnJoined");
                    state = STATES.UNJOINED;
                }
                break;
            case STATES.READY:
                break;
        }
	}
}
