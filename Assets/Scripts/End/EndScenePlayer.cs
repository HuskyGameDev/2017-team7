using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndScenePlayerState
{
    INACTIVE,
    SELECTING,
    CONTINUE,
    REMATCH
}

public class EndScenePlayer : MonoBehaviour {

    public int place;
    private int playerNumber;
    private EndScenePlayerState state;

    public SpriteRenderer playerCoin;
    public SpriteRenderer characterImage;

    public EndSceneController endSceneController;

    Controller controller;
    // Use this for initialization
    void Start()
    {
        playerNumber = EndData.instance.completionOrder[place - 1];
        BarnoutPlayer p = PlayerData.instance.barnoutPlayers[playerNumber - 1];
        if (p.IsActive())
        {
            state = EndScenePlayerState.SELECTING;
            controller = Inputs.GetController(playerNumber);
        }
        else
        {
            state = EndScenePlayerState.INACTIVE;
            gameObject.SetActive(false);
        }

        if (place == 1)
        {
            characterImage.sprite = endSceneController.GetWinnerSprite(p.GetCharacter());
        }
        else
        {
            characterImage.sprite = endSceneController.GetLoserCoin(p.GetCharacter());
        }
        playerCoin.sprite = endSceneController.GetPlayerCoin(playerNumber);
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == EndScenePlayerState.SELECTING)
        {
            if (controller.GetA())
            {
                state = EndScenePlayerState.CONTINUE;
            }
            if (controller.GetY())
            {
                state = EndScenePlayerState.REMATCH;
            }
        }
	}

    public EndScenePlayerState GetState()
    {
        return state;
    }

}
