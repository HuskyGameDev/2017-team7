using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEmPlayer : MinigamePlayer {

    public TuneEm minigame;

    bool failed = false;
    bool pressed;

    bool aBuffer = true;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!pressed)
        {
            if (controller.GetA() && aBuffer)
            {
                aBuffer = false;
                pressed = true;
                if (minigame.InBounds())
                {
                    Success();
                }
                else
                {
                    Fail();
                }
            }
            else
            {
                aBuffer = true;
            }
        }
        
	}

    void Success()
    {
        //animator.SetTrigger("Crank");
        score++;
    }
    public void Fail()
    {
        failed = true;
        if (pressed) score += minigame.GetCloseScore();
        //animator.SetTrigger("Fail");
    }

    public bool HasPressed()
    {
        return pressed;
    }
    public void Reset()
    {
        if (!HasFailed())
        {
            pressed = false;
        }
        

    }
    public bool HasFailed()
    {
        return failed;
    }
}
