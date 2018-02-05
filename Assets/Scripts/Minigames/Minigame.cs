using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame {
	string description;
	string sceneName;

	abstract public void DoMinigame();
	public virtual void SetPlayerStandings(){
		
	}
}
