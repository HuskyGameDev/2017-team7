using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEm : Minigame {

    float lowBound;
    float highBound;
    float linePos;
    float step = 0.0025f;
    bool done = false;

    public float stepIncr;

    bool started = false;

    protected override void InitMinigame()
    {
        started = true;
    }

    public override bool HasEnded()
    {
        return done;
    }

    // Use this for initialization
    void Start () {
        ResetRound();
	}

    public override void Tick()
    {
        linePos += step;
        if (linePos >= 1)
        {
            ElimUnpressed();
            ResetRound();
            CheckIfDone();
        }
    }

    public bool InBounds()
    {
        return linePos >= lowBound && linePos <= highBound;
    }

    void ElimUnpressed()
    {
        foreach(TuneEmPlayer p in players)
        {
            p.ElimIfUnpressed();
        }
    }

    void ResetRound()
    {

        lowBound = UnityEngine.Random.Range(0.5f, 0.7f);
        highBound = lowBound + UnityEngine.Random.Range(0.1f, 0.3f);

        //Debug.Log(highBound - lowBound);

        linePos = 0;
        step += stepIncr;// = Mathf.Min(0.01f, step * 1.05f); //increase step speed
        foreach(TuneEmPlayer p in players)
        {
            p.ResetPlayer();
        }



    }

    void CheckIfDone()
    {
        bool check = true;
        foreach(TuneEmPlayer p in players)
        {
            if (p.IsActive() && !p.HasFailed()) check = false;
        }
        done = check;
    }

    public float GetCloseScore()
    {
        return 1 - Mathf.Min(Mathf.Abs(highBound - linePos), Mathf.Abs(lowBound - linePos));
    }

    public float GetLinePos()
    {
        return linePos;
    }
    public float[] GetBounds()
    {
        return new float[2] { lowBound, highBound };
    }
}
