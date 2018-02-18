using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameController : MonoBehaviour {
	Minigame minigame;
	
	void Start () {
		minigame.InitMinigame();
	}
	
	void FixedUpdate () {
		minigame.Tick();
		if(minigame.End()){
			EndCode();
		}
	}
	/* Ending code, switches to next scene; allows users to pick powerups maybe? */
	void EndCode(){
		/* TODO: Probably the incorrect thing to do, but it's good for now */
		if(MinigameData.minigamesLeft > 0){
			Barnout.ChangeScene(MinigamePool.ChooseMinigame().sceneName);
		}else{
			/* TODO FIXME This only loads scene 0 after all minigames are done, but should choose a random map from all of the available ones. */
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
	}

	public void SetMinigame(Minigame m){
		minigame = m;
	}
}
