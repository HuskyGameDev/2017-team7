using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePumpItUp : Minigame {
    public int pumpsNeeded;
    public float timeGiven;
    bool done = false;
    Coroutine timer;
    bool begun = false;

    public Sprite[] armImages;

    protected override void InitMinigame()
    {
        timer = StartCoroutine(BeginTimer());
    }

    public override bool HasEnded()
    {
        //UNCOMMENT WHEN READY
        return done; 
    }
	
	
	public override void Tick ()
    {
        if (!done)
        {
            foreach (MinigamePumpItUpPlayer p in players)
            {
                if (p.GetScore() >= pumpsNeeded)
                {
                    StopCoroutine(timer);
                    Done();
                }
            }
        }
	}


    IEnumerator BeginTimer()
    {
        yield return new WaitForSecondsRealtime(timeGiven);
        Debug.Log("Timer done");
        Done();
    }

    void Done()
    {
        Debug.Log("Finished");
        done = true;
    }
}
