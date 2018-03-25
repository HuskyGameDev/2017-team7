using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameController : MonoBehaviour {
	Minigame minigame;
	private bool minigameStarted;
	void Start () {
		/*minigame.InitMinigame();*/
	}
	
	void FixedUpdate () {
		if(!minigameStarted){
			minigameStarted = true;
			minigame.BeginMinigame();
		}
		if(minigame.End()){
			EndCode();
		}
	}
	/* Ending code, switches to next scene; allows users to pick powerups maybe? */
	void EndCode(){
        /* TODO: Probably the incorrect thing to do, but it's good for now 
		  (Don't immediately load another minigame)
		*/
        MinigameData.standings = minigame.GetPlayerStandings();

		if(MinigameData.minigamesLeft > 0){
			Barnout.ChangeScene(MinigamePool.Instance.ChooseMinigame().sceneName); 
		}else{
			/* TODO FIXME This only loads scene 0 after all minigames are done, but should choose a 
			random racing map from all of the available ones. */
			Barnout.ChangeScene("MainMenu");
		}
	}

	public void SetMinigame(Minigame m){
		minigame = m;
	}
}
