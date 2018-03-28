using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameController : MonoBehaviour {
    public MinigameIntro intro;
    Minigame minigame;
	private bool minigameStarted;
    public bool skipIntro;
    private bool ended = false;
	void Start () {
		if (skipIntro)
        {
            intro.GetComponent<Animator>().SetTrigger("SkipIntro");
            StartMinigame();
        }
	}
	
	void FixedUpdate () {
		if(intro.IsDone() && !minigameStarted){
            StartMinigame();
		}
        if (minigameStarted)
        {
            minigame.Tick();
        }
		if(minigame.HasEnded() && !ended){
			EndCode();
		}
	}

    private void StartMinigame()
    {
        minigameStarted = true;
		minigame.BeginMinigame();
    }
	/* Ending code, switches to next scene; allows users to pick powerups maybe? */
	void EndCode(){
        /* TODO: Probably the incorrect thing to do, but it's good for now 
		  (Don't immediately load another minigame)
		*/
        ended = true;
        minigame.Finish();
        intro.StartOutro();

        MinigameData.standings = minigame.GetPlayerStandings();

		//if(MinigameData.minigamesLeft > 0){
		//	Barnout.ChangeScene(MinigamePool.Instance.ChooseMinigame().sceneName); 
		//}else{
		//	/* TODO FIXME This only loads scene 0 after all minigames are done, but should choose a 
		//	random racing map from all of the available ones. */
		//	Barnout.ChangeScene("MainMenu");
		//}
	}

	public void SetMinigame(Minigame m){
		minigame = m;
	}
}
