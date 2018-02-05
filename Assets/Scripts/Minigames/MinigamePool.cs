using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinigamePool {
	//all available minigames
	private static List<Minigame> minigames = new List<Minigame>();
	//all unchosen minigames
	private static List<Minigame> minigamePool = null;

	public static void RegisterMinigame(Minigame m){
		minigames.Add(m);
	}

	public static void ResetPool(){
		minigamePool = new List<Minigame>(minigames);
	}

	public static Minigame ChooseMinigame(){
		int index = Random.Range(0, minigamePool.Count);
		Minigame chosen = minigamePool[index];
		minigamePool.RemoveAt(index);
		return chosen;
	}
}
