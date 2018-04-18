using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEmPlayer : MinigamePlayer {

    public TuneEm minigame;

    public TuneEmBG bg;

    public TuneEm_AudioMaster audioMaster;

    bool failed = false;
    bool pressed = false;

    bool aBuffer = true;

    Animator animator;

    protected override void ToReady()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsActive", isActive);
    }

    // Use this for initialization
    protected override void Init ()
    {
        //bg.Init(isActive);
	}
	
	// Update is called once per frame
	protected override void Tick ()
    {
        if (!aBuffer)
        {
            if (!controller.GetA())
            {
                aBuffer = true;
            }
        }
        if (!pressed && !failed)
        {
            if (controller.GetA() && aBuffer)
            {
                aBuffer = false;
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

    protected override void OnGameDone()
    {
        
    }


    void Success()
    {
        audioMaster.PlayWrench();
        animator.SetTrigger("Crank");
        bg.Increment();
        score++;
    }
    void Fail()
    {
        failed = true;
        if (pressed) score += minigame.GetCloseScore();
        animator.SetTrigger("Fail");
        Debug.Log("Player " + playerNum + " failed: Score: " + score);
    }

    public void ElimIfUnpressed()
    {
        if (isActive && !pressed && !failed) Fail();
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

    public void WrenchFail()
    {
        audioMaster.PlayWrenchFail();
    }
}
