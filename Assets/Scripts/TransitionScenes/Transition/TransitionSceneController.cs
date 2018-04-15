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
        
        //Get 4 random powerups
        System.Random random = new System.Random();
        List<PowerupType> values = new List<PowerupType>((PowerupType[])Enum.GetValues(typeof(PowerupType)));
    }

    public void FixedUpdate()
    {
        switch (state)
        {
            case STATES.CHOOSING_POWERUP:


                break;
            case STATES.CHOOSING_SLOT:


                break;
        }
    }


    public void ToChoosing()
    {
        state = STATES.CHOOSING_POWERUP;
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
