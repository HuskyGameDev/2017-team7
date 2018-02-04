using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EEL : Powerup {

	// Use this for initialization
	void Start () {
		currentCooldown = 0;
	}
	
	public override bool UsePowerup() {
		if(base.UsePowerup()) {
			
			//Change player image to the one with the eels surrounding the kart here.

			//Detect that another kart has hit the eels
			



			Debug.Log("PLAYER " + owner.playerNumber + " USED EEL POWERUP, " + uses + " USES ARE LEFT.");
			return true;
		}else{
			return false;
		}
	}

	new void FixedUpdate () {
		base.FixedUpdate();
	}
}
