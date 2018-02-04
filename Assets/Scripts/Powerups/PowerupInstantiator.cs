using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PowerupType {
	FROG,
	EEL,
	SQUID,
	EAGLE,
	PUFFERFISH,
	CHICKEN,
	SPEEDBOOST
};

public class PowerupInstantiator : MonoBehaviour {
	public Powerup GetPowerup(PowerupType type, Player owner){
		Powerup p;
		switch(type){

			case PowerupType.EEL:
				p = Instantiate(GetComponent<SpeedBoost>());
				p.SetOwner(owner);
				return p;
			
			case PowerupType.SPEEDBOOST:
				p = Instantiate(GetComponent<SpeedBoost>());
				p.SetOwner(owner);
				return p;

			default:
				p = Instantiate(GetComponent<TestPowerup>()); 
				p.SetOwner(owner);
				return p;
		}
	}
}
