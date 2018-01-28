﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour {
	
	protected int uses = 1;
	public int cooldownTicks = 0;
	protected int currentCooldown = 0;
	protected Player owner = null;

	public bool CanUse(){
		return uses > 0 && currentCooldown <= 0;
	}
	/*
		This is the base fixed update. Should be called first in all derived classes FixedUpdate. 
	
		Decrements the amount of cooldown left.
	*/
	protected void FixedUpdate(){
		currentCooldown = currentCooldown == 0 ? 0 : currentCooldown - 1;
	}

	/* 
	Returns true if the powerup is used, false if it cannot for some reason. Again, should be called
	first thing in all derived classes. 
	
	This calculates the base effect of powerups, like how the uses go down and reseting the cooldown.
	*/
	protected bool UsePowerup(){
		if(!CanUse()){
			return false;
		}
		uses--;
		currentCooldown = cooldownTicks;
		return true;
	}

	/* Ratio between 0 and 1, teling how close to being able to be used again the powerup is.
		0 = can be used again,
		1 = was just used and needs to cooldown fully.
	 */
	public float CooldownRatio(){
		return (float)currentCooldown / cooldownTicks;
	}

}
