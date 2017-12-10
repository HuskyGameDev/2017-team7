using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class CharSelection : MonoBehaviour {

	public int playerNumber;
    public Image image;
    public Text charName;
    public Text pressStart;
    public Image darkenPanel;
    public CharSelectController controller;

    private enum STATES { NONE, SELECTING, SELECTED };

    private STATES state = STATES.NONE;

    private int character = -1;

    private bool axisBuffer;


    PlayerIndex playerIndex;
    GamePadState gp_state;
    GamePadState prev_gp_state;

    void Start()
    {
        image.sprite = null;
        playerIndex = (PlayerIndex)(playerNumber - 1);
    }
    void Update () {
        prev_gp_state = gp_state;
        gp_state = GamePad.GetState(playerIndex);
        switch (state)
        {
            case STATES.NONE:
                charName.gameObject.SetActive(false);
                pressStart.gameObject.SetActive(true);
                image.gameObject.SetActive(false);
                darkenPanel.gameObject.SetActive(true);
                if (gp_state.IsConnected)
                {
                    pressStart.text = "Press Start to Join!";
                    if (gp_state.Buttons.Start == ButtonState.Pressed && prev_gp_state.Buttons.Start == ButtonState.Released)
                    {
                        state = STATES.SELECTING;
                        character = controller.nextChar(-1);
                        image.sprite = controller.getCharImage(character);
                    }
                }
                else
                {
                    pressStart.text = "No controller connected.";
                }
                
                break;
            case STATES.SELECTING:
                charName.gameObject.SetActive(true);
                charName.text = controller.getCharName(character);
                pressStart.gameObject.SetActive(false);
                image.gameObject.SetActive(true);
                darkenPanel.gameObject.SetActive(controller.charSelected(character));
                if (gp_state.ThumbSticks.Left.X < 0 && axisBuffer)
                {
                    character = controller.prevChar(character);
                    image.sprite = controller.getCharImage(character);
                    darkenPanel.gameObject.SetActive(controller.charSelected(character));
                    axisBuffer = false;
                }
                if (gp_state.ThumbSticks.Left.X > 0 && axisBuffer)
                {
                    character = controller.nextChar(character);
                    image.sprite = controller.getCharImage(character);
                    darkenPanel.gameObject.SetActive(controller.charSelected(character));
                    axisBuffer = false;
                }
                if (gp_state.Buttons.A == ButtonState.Pressed && prev_gp_state.Buttons.A == ButtonState.Released)
                {
                    if (!controller.charSelected(character))
                    {
                        controller.selectCharacter(playerNumber - 1, character);
                        image.sprite = controller.getCharImage(character);
                        state = STATES.SELECTED;
                    }
                    
                }
                if (gp_state.Buttons.B == ButtonState.Pressed && prev_gp_state.Buttons.B == ButtonState.Released)
                {
                    character = -1;
                    state = STATES.NONE;
                }
                break;
            case STATES.SELECTED:
                pressStart.gameObject.SetActive(true);
                darkenPanel.gameObject.SetActive(true);
                pressStart.text = "READY";
                if (gp_state.Buttons.B == ButtonState.Pressed && prev_gp_state.Buttons.B == ButtonState.Released) //b
                {
                    controller.deselectCharacter(playerNumber - 1);
                    state = STATES.SELECTING;
                }
                if (gp_state.Buttons.Start == ButtonState.Pressed && prev_gp_state.Buttons.Start == ButtonState.Released) //start
                {
                    controller.startGame();
                }
                break;
        }	
        
        if (gp_state.ThumbSticks.Left.X == 0)
        {
            axisBuffer = true;
        }	
	}

    public int getSelChar()
    {
        return character;
    }
}
