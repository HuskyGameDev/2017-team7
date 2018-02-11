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
			//.....

			//Activate the EELtrigger script that incapacitates the player that collided with the player who has the eel powerup running. Is this done by instantiating an object?
			
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
