using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePool {

    private static MinigamePool instance = null;
	/*Only create once. Use ResetPool to reset the pool.*/
	public static MinigamePool Instance {
		get {
			if(instance == null){
                instance = new MinigamePool();
			}
			return instance;
		}
	}

    MinigamePool()
    {
		Debug.Log("Starting up minigame pool...");
        minigames = new List<MinigameMetadata>();
        minigamePool = null;
        current = null;
		/* 
		Load defined minigames
		*/
		LoadJSON();
        ResetPool();
    }


	//all available minigames
	private List<MinigameMetadata> minigames;
	//all unchosen minigames
	private List<MinigameMetadata> minigamePool;

	private MinigameMetadata current;

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

	private void LoadJSON(){
		Object[] resources = Resources.LoadAll("MinigameMetadata", typeof(TextAsset));
		
		Debug.Log("Found " + resources.Length + " resources.");

		foreach(TextAsset resource in resources){
			minigames.Add(JsonUtility.FromJson<MinigameMetadata>(resource.text));
			Debug.Log("Loaded Minigame " + minigames[minigames.Count - 1].description);
		}
	}

}
