using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPowerup : Powerup {

	// Use this for initialization
	void Start () {
		/* Set up all the junk about  uses and cooldown*/
		uses = 2;
		cooldownTicks = 300;
		currentCooldown = 0;
	}
	
	public override bool UsePowerup() {
		if(base.UsePowerup()) {
			/* PUT EFFECTS OF POWERUP HERE */
			Debug.Log("PLAYER " + owner.playerNumber + " USED POWERUP, " + uses + " USES ARE LEFT.");
			return true;
		}else{
			return false;
		}
	}

	new void FixedUpdate () {
		base.FixedUpdate();
	}
}
