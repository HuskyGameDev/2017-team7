using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour {

    bool raceFinished = false;
    private int numPlayers;

    public GameObject horizontalDivider;

    Animator animator;
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
        if (EndData.instance.raceDone)
        {
            raceFinished = true;
            animator.SetTrigger("Done");
        }
    }
}
