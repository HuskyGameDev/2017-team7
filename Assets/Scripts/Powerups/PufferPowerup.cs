using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferPowerup : Powerup {

    public PufferObject puffer;
    private PufferObject newPuffer;

    // Use this for initialization
    void Start()
    {
        uses = 3;
        currentCooldown = 0;
    }
    
    public override bool UsePowerup()
    {
        if (base.UsePowerup())
        {
            newPuffer = Instantiate(puffer);
            newPuffer.transform.SetPositionAndRotation(new Vector3(owner.playerRB.position.x, owner.playerRB.position.y, 0), owner.playerRB.transform.rotation);
            newPuffer.owner = owner;
            Debug.Log("PLAYER " + owner.playerNumber + " USED PUFFERFISH POWERUP, " + uses + " USES ARE LEFT.");
            return true;
        }
        else
        {
            return false;
        }
    }
}
