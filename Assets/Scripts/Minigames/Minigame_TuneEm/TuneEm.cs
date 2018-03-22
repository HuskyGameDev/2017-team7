using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEm : Minigame {

    float lowBound;
    float highBound;
    float linePos;
    float step;
    bool done = false;

    public override void BeginMinigame()
    {
        
    }

    public override bool End()
    {
        return done;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        linePos += step;
        if (linePos >= 1)
        {
            ElimUnpressed();
            Reset();
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
            if (!p.HasPressed())
            {
                p.Fail();
            }
        }
    }

    void Reset()
    {

       

        linePos = 0;
        step *= Mathf.Min(0.05f, 1.01f); //increase step speed
        foreach(TuneEmPlayer p in players)
        {
            p.Reset();
        }



    }

    void CheckIfDone()
    {
        //foreach()
    }

    public float GetCloseScore()
    {
        return 1 - Mathf.Min(Mathf.Abs(highBound - linePos), Mathf.Abs(lowBound - linePos));
    }

    public float GetLinePos()
    {
        return linePos;
    }
}
