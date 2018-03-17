using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePool {

    public static MinigamePool instance = new MinigamePool();
    public static void Instantiate()
    {
        instance = new MinigamePool();
    }

    MinigamePool()
    {
        minigames = new List<MinigameMetadata>();
        minigamePool = null;
        current = null;
    }


	//all available minigames
	private List<MinigameMetadata> minigames;
	//all unchosen minigames
	private List<MinigameMetadata> minigamePool;

	private MinigameMetadata current;
/*
	public static void RegisterAllMinigames(){
		minigames.Add(new MinigameTuneEm());
		//TODO put extra minigames here
	}

	public static void RegisterMinigame(MinigameMetadata m){
		minigames.Add(m);
	}
 */
	public void ResetPool(){
		minigamePool = new List<MinigameMetadata>(minigames);
		/* TODO: make this adjustable (maybe) */
		MinigameData.minigamesLeft = (int)(1.5*PlayerData.instance.numPlayers);
	}

	public MinigameMetadata ChooseMinigame(){
		int index = Random.Range(0, minigamePool.Count);
		current = minigamePool[index];
		minigamePool.RemoveAt(index);
		MinigameData.minigamesLeft--;
		return current;
	}

	public MinigameMetadata GetCurrentMinigameMetadata(){
		return current;
	}

}
