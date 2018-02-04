using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaglePowerup : Powerup
{

    public int useTimeTicks = 300;
    private int currentUseTime = -1;

    // Use this for initialization
    void Start()
    {
        currentCooldown = 0;
    }

    public override bool UsePowerup()
    {
        if (base.UsePowerup())
        {
            /* PUT EFFECTS OF POWERUP HERE */
            Debug.Log("PLAYER " + owner.playerNumber + " USED EAGLE, " + uses + " USES ARE LEFT.");
            owner.SetIsFlying(true);
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
            owner.SetIsFlying(false);
            currentUseTime = currentUseTime - 1;
        }
        else
        {
            currentUseTime = currentUseTime - 1;
        }
    }
}
