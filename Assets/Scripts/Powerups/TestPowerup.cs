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
	
	new public bool UsePowerup() {
		if(base.UsePowerup()) {
			/* PUT EFFECTS OF POWERUP HERE */
			Debug.Log("USED POWERUP, " + uses + " POWERUPS ARE LEFT.");
			return true;
		}else{
			return false;
		}
	}

	void Update() {

	}

	new void FixedUpdate () {
		base.FixedUpdate();
		/* This should all be in the player class or some other startup class.
			It's only here for testing purposes.
		 */
		owner = PlayerData.GetActivePlayers()[0];
		if(owner != null){
			if(Inputs.GetController(owner.playerNumber).GetA()){
				UsePowerup();
			}
		}
	}
}
