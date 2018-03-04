using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    public Player player;
    public Animator LapPanel;
    public Animator PosPanel;
    int place;
    int lap;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        LapDisplayMaster lapDisplayMaster = player.players.lapDisplayMaster;
        place = lapDisplayMaster.GetPlayerPosition(player.playerNumber);
        lap = lapDisplayMaster.GetPlayerLap(player.playerNumber);
        LapPanel.SetInteger("Lap", lap);
        PosPanel.SetInteger("Position", place);
	}
}
