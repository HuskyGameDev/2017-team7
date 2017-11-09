using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour {

    public Text speedText;
    public Player thisPlayer;

	// Use this for initialization
	void Start () {
        speedText.text = "Speed: ###";
    }
	
	// Update is called once per frame
	void Update () {
       speedText.text = "Speed: " + Mathf.Ceil(thisPlayer.playerRB.velocity.magnitude);
    }
}
