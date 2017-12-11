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
            nop4image.SetActive(false);
            p3laps.SetActive(false);
            p3place.SetActive(false);
            p4laps.SetActive(false);
            p4place.SetActive(false);
            p1place.GetComponent<RectTransform>().anchorMin = new Vector2(0.35f, 0);
            p1place.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.1f);
            p2place.GetComponent<RectTransform>().anchorMin = new Vector2(0.85f, 0);
            p2place.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.1f);
            p1cam.rect = new Rect(0, 0, 0.5f, 1);
            p2cam.rect = new Rect(0.5f, 0, 0.5f, 1);
        }
        else if (numPlayers == 3)
        {
            p4cam.rect = new Rect(0, 0, 0, 0);
            p4laps.SetActive(false);
            p4place.SetActive(false);
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
