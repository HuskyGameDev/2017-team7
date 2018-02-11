using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour{

    MinigameMetadata metadata;
    public virtual void Start(){
        metadata = MinigamePool.GetCurrentMinigame();
        GetComponent<MinigameController>().SetMinigame(this);
    }

    public abstract void InitMinigame();
	public abstract void BeginMinigame();

    public virtual void SetPlayerStandings(){
		MinigameData.standings = new int[4];
		MinigameData.standings[0] = 0;
		MinigameData.standings[1] = 1;
		MinigameData.standings[2] = 2;
		MinigameData.standings[3] = 3;
	}

}