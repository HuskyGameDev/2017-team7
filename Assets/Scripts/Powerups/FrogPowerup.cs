using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogPowerup : Powerup {

    public FrogObject frog;
    private FrogObject newFrog;

	// Use this for initialization
	void Start () {
        uses = 3;
        currentCooldown = 0;
    }

    public override bool UsePowerup()
    {
        if (base.UsePowerup())
        {
            /* PUT EFFECTS OF POWERUP HERE */


            newFrog = Instantiate(frog);
            newFrog.transform.SetPositionAndRotation(new Vector3(owner.playerRB.position.x, owner.playerRB.position.y, 0), owner.playerRB.transform.rotation);
            newFrog.rotation = owner.playerRB.rotation;
            newFrog.projectileRB.velocity = new Vector2(newFrog.rotation = owner.playerRB.velocity.x * 2, newFrog.rotation = owner.playerRB.velocity.y * 2);

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
