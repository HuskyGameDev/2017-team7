using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEmPlayer : MinigamePlayer {

    public TuneEm minigame;

    bool failed = false;
    bool pressed = false;

    bool aBuffer = true;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	protected override void Tick ()
    {
        //if ()
        if (!pressed)
        {
            if (controller.GetA())
            {
                //Debug.Log("Pressed A");
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
        }
        
	}

    void Success()
    {
        Debug.Log("Player " + playerNum + " got it!");
        //animator.SetTrigger("Crank");
        score++;
    }
    void Fail()
    {
        Debug.Log("Player " + playerNum + " failed");
        failed = true;
        if (pressed) score += minigame.GetCloseScore();
        //animator.SetTrigger("Fail");
    }

    public void ElimIfUnpressed()
    {
        if (isActive && !pressed) Fail();
    }

    public bool HasPressed()
    {
        return pressed;
    }
    public void ResetPlayer()
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
