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

public enum PowerupDirection
{
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 3
}

public class PowerupInstantiator : MonoBehaviour {
	public Powerup GetPowerup(BarnoutPowerup powerup, Player owner){
		Powerup p;
        if (powerup == null)
        {
            p = Instantiate(GetComponent<TestPowerup>());
            p.SetOwner(owner);
            p.uses = 0;
            return p;
        }
            
		switch(powerup.GetPowerup()){

			case PowerupType.EEL:
				p = Instantiate(GetComponent<EelPowerup>());
				p.SetOwner(owner);
                p.uses = powerup.GetUses();
				return p;
			case PowerupType.SPEEDBOOST:
				p = Instantiate(GetComponent<SpeedBoost>());
				p.SetOwner(owner);
                p.uses = powerup.GetUses();
                return p;
            case PowerupType.SQUID:
                p = Instantiate(GetComponent<OilPowerup>());
                p.SetOwner(owner);
                p.uses = powerup.GetUses();
                return p;
            case PowerupType.EAGLE:
                p = Instantiate(GetComponent<EaglePowerup>());
                p.SetOwner(owner);
                p.uses = powerup.GetUses();
                return p;
            case PowerupType.FROG:
                p = Instantiate(GetComponent<FrogPowerup>());
                p.SetOwner(owner);
                p.uses = powerup.GetUses();
                return p;
            case PowerupType.CHICKEN:
                p = Instantiate(GetComponent<ChickenPowerup>());
                p.SetOwner(owner);
                p.uses = powerup.GetUses();
                return p;
            case PowerupType.PUFFERFISH:
                p = Instantiate(GetComponent<PufferPowerup>());
                p.SetOwner(owner);
                p.uses = powerup.GetUses();
                return p;
            default:
				p = Instantiate(GetComponent<TestPowerup>()); 
				p.SetOwner(owner);
                p.uses = 0;
				return p;
		}
	}
}
