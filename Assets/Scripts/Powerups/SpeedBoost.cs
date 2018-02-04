﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : Powerup {

	// Use this for initialization
	void Start () {
		currentCooldown = 0;
	}
	
	public override bool UsePowerup() {
		if(base.UsePowerup()) {
			
			owner.SetBoost(Player.BOOSTS.BOOST_PAD, 1);
			owner.state = Player.STATES.BOOST;

			Debug.Log("PLAYER " + owner.playerNumber + " USED SPEEDBOOST, " + uses + " USES ARE LEFT.");
			return true;
		}else{
			return false;
		}
	}

	new void FixedUpdate () {
		base.FixedUpdate();
	}

}

