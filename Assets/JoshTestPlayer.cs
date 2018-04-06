using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player for dodgeball game.

public class JoshTestPlayer : MinigamePlayer {

    public int Lives { get; set; }

    protected override void ToReady()
    {
        
    }

    protected override void Init()
    {
        score = 4;
    }

    protected override void Tick()
    {
        
    }

    protected override void OnGameDone()
    {
        
    }

    public void SetScore(int s)
    {
        score = s;
    }
}
