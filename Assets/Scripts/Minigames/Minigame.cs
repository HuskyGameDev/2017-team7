using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour{

    MinigameMetadata metadata;
    public void Awake(){
        metadata = MinigamePool.GetCurrentMinigameMetadata();
        GetComponent<MinigameController>().SetMinigame(this);
    }
    
    /* 
    Implement this with the initialization code for your minigame.
    Minigame starts when this is called.
     */
    public abstract void BeginMinigame();
    
    
    /* returns whether the minigame has ended or not. */
    public abstract bool End();

    /* Sets MinigameData.standings */
    public virtual void SetPlayerStandings(){
        List<MinigameData.Standing> standings = new List<MinigameData.Standing>();
        for(int i = 0; i < 4; i++){
            MinigameData.Standing standing;
            standing.playerNumber = i+1;
            standing.standing = i+1;
            standings.Add(standing);
        }
        MinigameData.standings = standings.ToArray();
	}

}