using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        image.sprite = null;
    }
    void Update () {
        switch (state)
        {
            case STATES.NONE:
                pressStart.text = "Press Start to Join!";
                charName.gameObject.SetActive(false);
                pressStart.gameObject.SetActive(true);
                image.gameObject.SetActive(false);
                darkenPanel.gameObject.SetActive(true);
                if (Input.GetKeyDown("joystick " + playerNumber + " button 7"))
                {
                    state = STATES.SELECTING;
                    character = controller.nextChar(-1);
                    image.sprite = controller.getCharImage(character);
                }
                break;
            case STATES.SELECTING:
                charName.gameObject.SetActive(true);
                charName.text = controller.getCharName(character);
                pressStart.gameObject.SetActive(false);
                image.gameObject.SetActive(true);
                darkenPanel.gameObject.SetActive(controller.charSelected(character));
                if (Input.GetAxis("LeftStickX-" + playerNumber) < 0 && axisBuffer)
                {
                    character = controller.prevChar(character);
                    image.sprite = controller.getCharImage(character);
                    darkenPanel.gameObject.SetActive(controller.charSelected(character));
                    axisBuffer = false;
                }
                if (Input.GetAxis("LeftStickX-" + playerNumber) > 0 && axisBuffer)
                {
                    character = controller.nextChar(character);
                    image.sprite = controller.getCharImage(character);
                    darkenPanel.gameObject.SetActive(controller.charSelected(character));
                    axisBuffer = false;
                }
                if (Input.GetKeyDown("joystick " + playerNumber + " button 0"))
                {
                    if (!controller.charSelected(character))
                    {
                        controller.selectCharacter(playerNumber - 1, character);
                        image.sprite = controller.getCharImage(character);
                        state = STATES.SELECTED;
                    }
                    
                }
                if (Input.GetKeyDown("joystick " + playerNumber + " button 1"))
                {
                    character = -1;
                    state = STATES.NONE;
                }
                break;
            case STATES.SELECTED:
                pressStart.gameObject.SetActive(true);
                darkenPanel.gameObject.SetActive(true);
                pressStart.text = "READY";
                if (Input.GetKeyDown("joystick " + playerNumber + " button 1")) //b
                {
                    controller.deselectCharacter(playerNumber - 1);
                    state = STATES.SELECTING;
                }
                if (Input.GetKeyDown("joystick " + playerNumber + " button 7")) //start
                {
                    controller.startGame();
                }
                break;
        }	
        
        if (Input.GetAxis("LeftStickX-" + playerNumber) == 0)
        {
            axisBuffer = true;
        }	
	}

    public int getSelChar()
    {
        return character;
    }
}
