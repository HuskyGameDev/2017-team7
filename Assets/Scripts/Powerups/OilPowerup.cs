using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPowerup : Powerup {

    public GameObject oil;
    private GameObject newOil;

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
            /* PUT EFFECTS OF POWERUP HERE */
            
        
            newOil = Instantiate(oil);
            newOil.transform.SetPositionAndRotation(new Vector3(owner.playerRB.position.x, owner.playerRB.position.y, 0), owner.playerRB.transform.rotation);

            Debug.Log("PLAYER " + owner.playerNumber + " USED OIL POWERUP, " + uses + " USES ARE LEFT.");
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
    }
}
