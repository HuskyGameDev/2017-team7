﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;


public class PlayerPanel : MonoBehaviour {

    private enum STATES { DISCONNECTED, UNJOINED, SELECTING, READY };
    private STATES state;

	public int playerNum;
    public CharSelectController charSelectController;

    public Text CharNameText;
    public Image CharImage;

    private int selChar = 0;

    private Animator animator;

    private MM_AudioMaster audioMaster;

    PlayerIndex playerIndex;
    public GamePadState gp_state;
    public GamePadState prev_gp_state;


	void Start () {
        audioMaster = FindObjectOfType<MM_AudioMaster>();
        animator = GetComponent<Animator>();
        state = STATES.DISCONNECTED;
        playerIndex = (PlayerIndex)(playerNum - 1);
    }
	





	void Update () {

        prev_gp_state = gp_state;
        gp_state = GamePad.GetState(playerIndex);

        if (!gp_state.IsConnected && state != STATES.DISCONNECTED)
        {
            state = STATES.DISCONNECTED;
            animator.SetTrigger("Disconnected");
            charSelectController.RemoveOn(selChar, playerNum);
            charSelectController.DeSelectChar(selChar, true);
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
                    audioMaster.PlaySelect();
                    animator.SetTrigger("Joined");
                    state = STATES.SELECTING;
                    CharPanel nextChar = charSelectController.NextChar(selChar - 1);
                    ApplyCharacter(nextChar);
                    
                }
                else if (Barnout.ButtonPressed(gp_state.Buttons.B, prev_gp_state.Buttons.B))
                {
                    audioMaster.PlaySelect();
                    charSelectController.BackToMainMenu();
                }
                break;
            case STATES.SELECTING:
                if (Barnout.ButtonPressed(gp_state.Buttons.B, prev_gp_state.Buttons.B))
                {
                    audioMaster.PlaySelect();
                    animator.SetTrigger("UnJoined");
                    state = STATES.UNJOINED;
                    charSelectController.RemoveOn(selChar, playerNum);
                }

                if (gp_state.ThumbSticks.Left.Y < -0.5 && prev_gp_state.ThumbSticks.Left.Y >= -0.5) //down
                {
                    audioMaster.PlayToggle();
                    CharPanel nextChar = charSelectController.NextChar(selChar);
                    ApplyCharacter(nextChar);
                }
                else if (gp_state.ThumbSticks.Left.Y > 0.5 && prev_gp_state.ThumbSticks.Left.Y <= 0.5) //up
                {
                    audioMaster.PlayToggle();
                    CharPanel prevChar = charSelectController.PrevChar(selChar);
                    ApplyCharacter(prevChar);
                    //TODO: go to one character up
                }
                
                if (Barnout.ButtonPressed(gp_state.Buttons.A, prev_gp_state.Buttons.A))
                {
                    if (charSelectController.SelectChar(selChar, playerNum))
                    {
                        animator.SetTrigger("Ready");
                        state = STATES.READY;
                    }
                }


                break;
            case STATES.READY:

                if (Barnout.ButtonPressed(gp_state.Buttons.B, prev_gp_state.Buttons.B))
                {
                    
                    animator.SetTrigger("CancelReady");
                    state = STATES.SELECTING;
                    charSelectController.DeSelectChar(selChar);
                }

                if (Barnout.ButtonPressed(gp_state.Buttons.Start, prev_gp_state.Buttons.Start))
                {
                    audioMaster.PlaySelect();
                    charSelectController.StartGame();
                }

                break;
        }
	}


    private void ApplyCharacter(CharPanel panel)
    {
        charSelectController.RemoveOn(selChar, playerNum);
        selChar = panel.Index;
        charSelectController.AddOn(selChar, playerNum);
        CharNameText.text = panel.CharName;
        CharImage.sprite = panel.Portrait;
    }

    public int CharOn()
    {
        return selChar;
    }

    public bool IsReady()
    {
        return state == STATES.READY;
    }
    public bool IsSelecting()
    {
        return state == STATES.SELECTING;
    }

    public int SelectedChar()
    {
        return selChar;
    }
}
