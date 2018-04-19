using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour {

    bool raceFinished = false;
    private int numPlayers;

    public GameObject horizontalDivider;

    Animator animator;
    bool done = false;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        //  speedText.text = "Speed: ###";
        numPlayers = PlayerData.instance.numPlayers;
        if (numPlayers == 2)
        {
            horizontalDivider.SetActive(false);
        }
        else
        {
            horizontalDivider.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (EndData.instance.raceDone && !done)
        {
            done = true;
            raceFinished = true;
            animator.SetInteger("NumPlayers", numPlayers);
            if (numPlayers > 2) animator.SetInteger("Winner", EndData.instance.completionOrder[0]);
            else
            {
                int first = EndData.instance.completionOrder[0];
                int second = EndData.instance.completionOrder[1];
                animator.SetInteger("Winner", (first < second) ? 1 : 2);
            }
            animator.SetTrigger("Done");
        }
    }
}
