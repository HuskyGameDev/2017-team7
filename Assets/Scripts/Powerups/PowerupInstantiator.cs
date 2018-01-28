using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PowerupType {
	FROG,
	EEL,
	SQUID,
	EAGLE,
	PUFFERFISH,
	CHICKEN
};

public class PowerupInstantiator : MonoBehaviour {
	public Powerup GetPowerup(PowerupType type, Player owner){
		switch(type){
			default:
				Powerup p = Instantiate(GetComponent<TestPowerup>()); 
				p.SetOwner(owner);
				return p;
		}
	}
}
