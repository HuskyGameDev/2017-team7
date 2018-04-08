using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelPowerup : Powerup {

    public GameObject[] EelObjects;

	IEnumerator endEel(float time) {
		yield return new WaitForSeconds(time);
		EelObjects[owner.playerNumber - 1].SetActive(false);
	}

	// Use this for initialization
	void Start () {

	}

	
	public override bool UsePowerup() {
		if(base.UsePowerup()) {
			
			//Change player image to the one with the eels surrounding the kart here.
			//.....

			EelObjects[owner.playerNumber - 1].SetActive(true);
			StartCoroutine(endEel(3f));
			
			
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
