using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePumpItUpPlayer : MinigamePlayer {

    bool up = false;
    Animator animator;

    protected override void ToReady()
    {
        animator = GetComponent<Animator>();
        if (isActive) animator.SetTrigger("ToActive");
    }

    protected override void Init ()
    {
        
	}
	
	protected override void Tick ()
    {
        
        if (up)
        {
            if (controller.GetLsYaxis() < -0.75)
            {
                up = false;
                score++;
                animator.SetBool("Up", up);
            }
        }
        else
        {
            if (controller.GetLsYaxis() > 0.75)
            {
                up = true;
                animator.SetBool("Up", up);
            }
        }
        
	}

    protected override void OnGameDone()
    {
        animator.SetTrigger("Finish");
    }
}
