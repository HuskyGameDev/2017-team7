using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePumpItUp : Minigame {
    public int pumpsNeeded;
    public float timeGiven;
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
                if (p.GetScore() >= pumpsNeeded)
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
    }
}
