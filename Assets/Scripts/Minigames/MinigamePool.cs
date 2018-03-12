using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinigamePool {
	//all available minigames
	private static List<MinigameMetadata> minigames = new List<MinigameMetadata>();
	//all unchosen minigames
	private static List<MinigameMetadata> minigamePool = null;

	private static MinigameMetadata current = null;
/*
	public static void RegisterAllMinigames(){
		minigames.Add(new MinigameTuneEm());
		//TODO put extra minigames here
	}

	public static void RegisterMinigame(MinigameMetadata m){
		minigames.Add(m);
	}
 */
	public static void ResetPool(){
		minigamePool = new List<MinigameMetadata>(minigames);
		/* TODO: make this adjustable (maybe) */
		MinigameData.minigamesLeft = (int)(1.5*PlayerData.instance.numPlayers);
	}

	public static MinigameMetadata ChooseMinigame(){
		int index = Random.Range(0, minigamePool.Count);
		current = minigamePool[index];
		minigamePool.RemoveAt(index);
		MinigameData.minigamesLeft--;
		return current;
	}

	public static MinigameMetadata GetCurrentMinigameMetadata(){
		return current;
	}

}
