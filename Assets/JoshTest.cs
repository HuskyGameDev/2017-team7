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
    private int playersOut = 0;

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
        foreach (Player p in playerCars.players)
        {
            if (p.state == Player.STATES.INCAPACITATED)
            {
                // ((JoshTestPlayer)players[p.playerNumber - 1]) is BAD!!!!! NOT RIGHT PLAYER
                ((JoshTestPlayer)players[p.playerNumber - 1]).Lives--;
                if (((JoshTestPlayer)players[p.playerNumber - 1]).Lives <= 0)
                {
                    ((JoshTestPlayer)players[p.playerNumber - 1]).SetScore(++playersOut);
                }
            }
        }

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
