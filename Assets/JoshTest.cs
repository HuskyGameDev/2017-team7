using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Test for a potential future dodgeball like gametype/minigame.
// Players start 27 X difference, 22 Y difference
// P1 85X 30Y

public class JoshTest : Minigame {

    public float timeGiven;
    private bool done = false;
    private Coroutine timer;
    public int lives;
    public Players playerCars;
    public int playersOut;

    protected override void InitMinigame()
    {
        playerCars.gameObject.SetActive(true);
        timer = StartCoroutine(BeginTimer());
        foreach (JoshTestPlayer p in players)
        {
            p.Lives = lives;
        }
    }

    public override void Tick()
    {
        if (playersOut == PlayerData.instance.numPlayers)
        {
            done = true;
        }
    }

    public override bool HasEnded()
    {
        return done;
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
