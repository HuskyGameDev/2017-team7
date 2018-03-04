using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenPowerup : Powerup {

	public GameObject chicken;
	private GameObject newChicken;

    void Start () {
		uses = 3;
		currentCooldown = 0;
	}
	
	public override bool UsePowerup() {
		if(base.UsePowerup()) {
			
			newChicken = Instantiate(chicken);
			newChicken.transform.SetPositionAndRotation(new Vector3(owner.playerRB.position.x, owner.playerRB.position.y, 0), owner.playerRB.transform.rotation);

			Debug.Log("PLAYER " + owner.playerNumber + " USED CHICKEN POWERUP, " + uses + " USES ARE LEFT.");
			return true;
		}else{
			return false;
		}
	}

	new void FixedUpdate () {
		base.FixedUpdate();
	}
}
