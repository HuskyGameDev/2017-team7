using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSceneController : MonoBehaviour {

    public TransitionSticks sticks;
    public TransitionCoins coins;
    public TransitionButtons buttons;
    public MT_AudioMaster audioMaster;

    private enum STATES { CHOOSING_POWERUP, CHOOSING_SLOT, WAITING }
    private STATES state = STATES.WAITING;
    Animator animator;

    private int currPlace = 0;

    private Controller currController;
    private MinigameData.Standing[] standings;
    private MinigameData.Standing currStanding;
    private BarnoutPlayer currPlayer;

    private BarnoutPowerup tempPowerup;

    public void Start()
    {
        audioMaster.PlayMusic();

        Debug.Log("Transition scene start");
        standings = MinigameData.standings;
        animator = GetComponent<Animator>();
        animator.SetInteger("NumPlayers", PlayerData.instance.numPlayers);
        //Get 4 random powerups

        //PlayerData.instance.barnoutPlayers[0].SetPowerup((int)PowerupDirection.DOWN, new BarnoutPowerup(PowerupType.CHICKEN, 3));
        //PlayerData.instance.barnoutPlayers[1].SetPowerup((int)PowerupDirection.LEFT, new BarnoutPowerup(PowerupType.CHICKEN, 3));

        coins.Init();
        sticks.Init();
        NextPlayer();
        Debug.Log("End Transition scene start");
    }

    public void FixedUpdate()
    {
        switch (state)
        {
            case STATES.CHOOSING_POWERUP:
                if (currController.GetLsXaxis() < -0.5) { ChoosePowerup(PowerupDirection.LEFT); }
                else if (currController.GetLsXaxis() > 0.5) { ChoosePowerup(PowerupDirection.RIGHT); }
                else if (currController.GetLsYaxis() < -0.5) { ChoosePowerup(PowerupDirection.DOWN); }
                else if (currController.GetLsYaxis() > 0.5) { ChoosePowerup(PowerupDirection.UP); }
                break;
            case STATES.CHOOSING_SLOT:
                if (currController.GetDPadRight()) { ChooseSlot(PowerupDirection.RIGHT); }
                else if (currController.GetDPadLeft()) { ChooseSlot(PowerupDirection.LEFT); }
                else if (currController.GetDPadDown()) { ChooseSlot(PowerupDirection.DOWN); }
                else if (currController.GetDPadUp()) { ChooseSlot(PowerupDirection.UP); }
                break;
        }
    }

    private void ChoosePowerup(PowerupDirection direction)
    {
        if (!sticks.IsTaken(direction))
        {
            tempPowerup = sticks.SetTaken(direction, coins.playerCoinImages[currStanding.playerNumber - 1]);
            state = STATES.CHOOSING_SLOT;
            animator.SetTrigger("ToChooseSlot");
        }
    }

    private void ChooseSlot(PowerupDirection direction)
    {
        if (currPlayer.GetPowerup((int) direction) == null)
        {
            state = STATES.WAITING;
            animator.SetTrigger("DoneChooseSlot");
            buttons.SetTaken(direction, tempPowerup.GetPowerup());
            PlayerData.instance.barnoutPlayers[currStanding.playerNumber - 1].SetPowerup((int)direction, tempPowerup);
        }   
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
                currPlayer = PlayerData.instance.barnoutPlayers[s.playerNumber - 1];
                buttons.InitPowerups(currPlayer);
            }
        }
        Debug.Log("Currplace: " + currPlace);
    }

    public void NextScene()
    {
        try
        {
            Barnout.ChangeScene(MinigamePool.Instance.ChooseMinigame().sceneName);
        }
        catch(ArgumentOutOfRangeException e)
        {
            Barnout.ChangeScene("FieldMap");
        }
            
        
    }
}
