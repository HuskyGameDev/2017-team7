using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour {

    public Text speedText;
    public Player thisPlayer;
    private int numPlayers;

    public GameObject horizontalDivider, p1place, p2place, p3laps, p3place, p4laps, p4place, nop4image;
    public Camera p1cam, p2cam, p3cam, p4cam;

	// Use this for initialization
	void Start () {

        //  speedText.text = "Speed: ###";
        numPlayers = PlayerData.GetActivePlayers().Length;
        if (numPlayers == 2)
        {
            horizontalDivider.SetActive(false);
            p1cam.rect = new Rect(0, 0, 0.5f, 1);
            p2cam.rect = new Rect(0.5f, 0, 0.5f, 1);
            nop4image.SetActive(false);
        }
        else if (numPlayers == 3)
        {
            p4cam.rect = new Rect(0, 0, 0, 0);
            
        }
        else if (numPlayers == 4)
        {
            nop4image.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        // speedText.text = "Speed: " + Mathf.Ceil(thisPlayer.playerRB.velocity.magnitude);
    }
}
