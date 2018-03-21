using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Minigame : MonoBehaviour{

    public MinigamePlayer [] players;

    MinigameMetadata metadata;
    public void Awake(){
        metadata = MinigamePool.instance.GetCurrentMinigameMetadata();
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
    public virtual MinigameData.Standing[] GetPlayerStandings(){
        List<MinigamePlayer> playerOrder = new List<MinigamePlayer>(players);
        playerOrder.Sort((p1, p2) => p1.CompareScore(p2));
        List<MinigameData.Standing> standings = new List<MinigameData.Standing>();
        for(int i = 0; i < playerOrder.Count; i++){
            MinigamePlayer p = playerOrder[i];
            MinigameData.Standing standing;
            standing.playerNumber = p.playerNum;
            standing.standing = i + 1;
            standings.Add(standing);
        }
        return standings.ToArray();
	}

}