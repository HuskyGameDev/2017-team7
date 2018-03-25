using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaglePowerup : Powerup
{

    public int useTimeTicks = 300;
    private int currentUseTime = -1;
    public const int flyingLayer = 12;
    private int minTime = 50;

    // Use this for initialization
    void Start()
    {
        currentCooldown = 0;
    }

    public override bool UsePowerup()
    {
        if (owner.GetPlayerState() == Player.STATES.FLYING && (useTimeTicks - currentUseTime) > minTime)
        {
            Debug.Log("The player removed the eagle.");
            owner.SetPlayerStateFlying(false);
            currentUseTime = -1;
            return true;
        }
        else if (base.UsePowerup() && owner.GetSpeedPercent() > 0.5)
        {
            /* PUT EFFECTS OF POWERUP HERE */
            Debug.Log("PLAYER " + owner.playerNumber + " USED EAGLE, " + uses + " USES ARE LEFT.");
            owner.SetPlayerStateFlying(true);
            currentUseTime = useTimeTicks;
            return true;
        }
        else
        {
            return false;
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (currentUseTime == 0)
        {
            Debug.Log("The eagle has left.");
            owner.SetPlayerStateFlying(false);
            currentUseTime = currentUseTime - 1;
        }
        else
        {
            currentUseTime = currentUseTime - 1;
        }
    }
}
