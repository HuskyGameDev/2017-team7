using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogPowerup : Powerup {

    public FrogObject frog;
    private FrogObject newFrog;

	// Use this for initialization
	void Start () {

    }

    public override bool UsePowerup()
    {
        if (base.UsePowerup())
        {
            /* PUT EFFECTS OF POWERUP HERE */


            newFrog = Instantiate(frog);
            newFrog.transform.SetPositionAndRotation(new Vector3(owner.playerRB.position.x, owner.playerRB.position.y, 0), owner.playerRB.transform.rotation);
            newFrog.rotation = owner.playerRB.rotation;
            newFrog.projectileRB.velocity = owner.transform.up * (300 + owner.playerRB.velocity.magnitude);
            newFrog.owner = owner;
            newFrog.toDestroy = true;

            Debug.Log("PLAYER " + owner.playerNumber + " USED FROG POWERUP, " + uses + " USES ARE LEFT.");
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}   
}
