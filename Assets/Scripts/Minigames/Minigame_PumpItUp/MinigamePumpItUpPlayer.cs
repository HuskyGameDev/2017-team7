using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePumpItUpPlayer : MinigamePlayer {

    bool up = false;
    Animator animator;
	// Use this for initialization
	void Start () {
        controller = Inputs.GetController(playerNum);
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        if (controller != null)
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
	}
}
