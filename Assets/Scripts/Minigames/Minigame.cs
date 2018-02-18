using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour{

    MinigameMetadata metadata;
    public void Awake(){
        metadata = MinigamePool.GetCurrentMinigameMetadata();
        GetComponent<MinigameController>().SetMinigame(this);
    }
    
    /* Implement this with the initialization code for your minigame */
    public abstract void InitMinigame();
    
    /* This code is calculated every tick of your mingame. Using FixedUpdate is fine, but using this method is preferred 
     as it forces correct ordering of execution.
    */
	public abstract void Tick();
    
    
    /* returns whether the minigame has ended or not. */
    public abstract bool End();

    /* Returns an array */
    public virtual void SetPlayerStandings(){
		MinigameData.standings = new int[4];
		MinigameData.standings[0] = 0;
		MinigameData.standings[1] = 1;
		MinigameData.standings[2] = 2;
		MinigameData.standings[3] = 3;
	}

}