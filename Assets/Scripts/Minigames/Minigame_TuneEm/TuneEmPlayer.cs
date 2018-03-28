using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEmPlayer : MinigamePlayer {

    public TuneEm minigame;

    public TuneEmBG bg;

    bool failed = false;
    bool pressed = false;

    bool aBuffer = true;

    Animator animator;

	// Use this for initialization
	protected override void Init () {
        animator = GetComponent<Animator>();
        animator.SetBool("IsActive", isActive);
        //bg.Init(isActive);
	}
	
	// Update is called once per frame
	protected override void Tick ()
    {
        if (!pressed && !failed)
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

    protected override void OnGameDone()
    {
        
    }


    void Success()
    {
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
}
