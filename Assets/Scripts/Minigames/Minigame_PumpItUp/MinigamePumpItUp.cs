using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePumpItUp : Minigame {
    public int pumpsNeeded;
    public float timeGiven;
    public MinigamePumpItUpPlayer[] players;
    bool done = false;
    Coroutine timer;

    public override void BeginMinigame()
    {
        timer = StartCoroutine(BeginTimer());
    }

    public override bool End()
    {
        //UNCOMMENT WHEN READY
        return false;// done; 
    }
	
	
	void FixedUpdate ()
    {
        if (!done)
        {
            foreach (MinigamePumpItUpPlayer p in players)
            {
                if (p.GetNumOfPumps() >= pumpsNeeded)
                {
                    StopCoroutine(timer);
                    Finish();
                }
            }
        }
	}


    IEnumerator BeginTimer()
    {
        yield return new WaitForSecondsRealtime(timeGiven);
        Debug.Log("Timer done");
        Finish();
    }

    void Finish()
    {
        done = true;
        Debug.Log("DONE");
        foreach (MinigamePumpItUpPlayer p in players)
        {
            Debug.Log("Player: " + p.playerNum + " Pumps: " + p.GetNumOfPumps());
        }
    }

    public override MinigameData.Standing[] GetPlayerStandings()
    {
        List<MinigamePumpItUpPlayer> playerOrder = new List<MinigamePumpItUpPlayer>();
        playerOrder.Sort((p1, p2) => p1.GetNumOfPumps().CompareTo(p2.GetNumOfPumps()));
        List<MinigameData.Standing> standings = new List<MinigameData.Standing>();
        for(int i = 0; i < playerOrder.Count; i++)
        {
            MinigamePumpItUpPlayer p = playerOrder[i];
            MinigameData.Standing standing;
            standing.playerNumber = p.playerNum;
            standing.standing = i;
            standings.Add(standing);
        }
        return standings.ToArray();
    }
}
