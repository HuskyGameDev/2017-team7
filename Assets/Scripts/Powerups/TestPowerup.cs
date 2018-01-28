using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPowerup : Powerup {

	// Use this for initialization
	void Start () {
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
