﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel : Powerup {

	public GameObject EelObject;

	IEnumerator endEel(float time) {
		yield return new WaitForSeconds(time);
		EelObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		currentCooldown = 0;
	}

	
	public override bool UsePowerup() {
		if(base.UsePowerup()) {
			
			//Change player image to the one with the eels surrounding the kart here.
			//.....

			EelObject.SetActive(true);

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
