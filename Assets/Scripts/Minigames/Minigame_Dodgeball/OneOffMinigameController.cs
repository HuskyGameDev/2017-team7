using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OneOffMinigameController : MonoBehaviour {
    public MinigameIntro intro;
    OneOffMinigame minigame;
    private bool minigameReady;
	private bool minigameStarted;
    public bool skipIntro;
    private bool ended = false;
	void Start () {
		if (skipIntro)
        {
            intro.SkipIntro();
            ReadyMinigame();
            StartMinigame();
        }
	}
	
	void FixedUpdate () {
        if (intro.IsReady() && !minigameReady)
        {
            ReadyMinigame();
        }
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

    private void ReadyMinigame()
    {
        minigameReady = true;
        minigame.ToReady();
    }

    private void StartMinigame()
    {
        minigameStarted = true;
		minigame.BeginMinigame();
    }
	/* Ending code, switches to next scene; allows users to pick powerups maybe? */
	void EndCode(){
        ended = true;
        minigame.Finish();
        intro.StartOutro();

		EndData.Instantiate();

        EndData.instance.completionOrder = new int[minigame.players.Length];
        EndData.instance.fromSecret = true;

		MinigameData.Standing[] standings = minigame.GetPlayerStandings();
		List<MinigameData.Standing> orderedStandings = standings.ToList().OrderBy(x => x.standing).ToList();
		for(int i = 0; i < orderedStandings.Count(); i++){
			EndData.instance.completionOrder[i] = orderedStandings[i].playerNumber;
		}

        Barnout.ChangeScene("EndScene");

	}

	public void SetMinigame(OneOffMinigame m){
		minigame = m;
	}
}