using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSceneController : MonoBehaviour {

    private enum STATES { CHOOSING_POWERUP, CHOOSING_SLOT, WAITING }
    private STATES state = STATES.WAITING;
    Animator animator;

    private int currPlayer;
    private int currPlace = 0;

    Controller currController;
    List<MinigameData.Standing> standings;
    MinigameData.Standing currStanding;

    public void Start()
    {
        standings = new List<MinigameData.Standing>(MinigameData.standings);
        animator = GetComponent<Animator>();
        animator.SetInteger("NumPlayers", PlayerData.instance.numPlayers);
        //Get 4 random powerups
        System.Random random = new System.Random();
        List<PowerupType> values = new List<PowerupType>((PowerupType[])Enum.GetValues(typeof(PowerupType)));
        NextPlayer();
    }

    public void FixedUpdate()
    {
        switch (state)
        {
            case STATES.CHOOSING_POWERUP:
                //right
                if (currController.GetLsXaxis() < -0.5)
                {
                    ChoosePowerup(PowerupDirection.RIGHT);
                }
                //left
                else if (currController.GetLsXaxis() > 0.5)
                {
                    ChoosePowerup(PowerupDirection.LEFT);
                }
                //down
                else if (currController.GetLsYaxis() < -0.5)
                {
                    ChoosePowerup(PowerupDirection.DOWN);
                }
                //up
                else if (currController.GetLsYaxis() > 0.5)
                {
                    ChoosePowerup(PowerupDirection.UP);
                }
                break;
            case STATES.CHOOSING_SLOT:
                //right
                if (currController.GetDPadRight())
                {
                    ChooseSlot(PowerupDirection.RIGHT);
                }
                //left
                else if (currController.GetDPadLeft())
                {
                    ChooseSlot(PowerupDirection.LEFT);
                }
                //down
                else if (currController.GetDPadDown())
                {
                    ChooseSlot(PowerupDirection.DOWN);
                }
                //up
                else if (currController.GetDPadUp())
                {
                    ChooseSlot(PowerupDirection.UP);
                }

                break;
        }
    }

    private void ChoosePowerup(PowerupDirection direction)
    {
        state = STATES.CHOOSING_SLOT;
        animator.SetTrigger("ToChooseSlot");
    }

    private void ChooseSlot(PowerupDirection direction)
    {
        state = STATES.WAITING;
        animator.SetTrigger("DoneChooseSlot");
    }

    public void ToChoosing()
    {
        state = STATES.CHOOSING_POWERUP;
        currController = Inputs.GetController(currStanding.playerNumber);
    }

    public void NextPlayer()
    {
        currPlace++;
        foreach(MinigameData.Standing s in standings)
        {
            if (s.standing == currPlace)
            {
                currStanding = s;
            }
        }
    }


}
