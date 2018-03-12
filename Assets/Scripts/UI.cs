using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour {

    private int numPlayers;

    public GameObject horizontalDivider;

	// Use this for initialization
	void Start () {

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
	void Update () {
        // speedText.text = "Speed: " + Mathf.Ceil(thisPlayer.playerRB.velocity.magnitude);
    }
}
