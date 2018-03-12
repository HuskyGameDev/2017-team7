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

    private bool raceDone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!raceDone)
        {
            if (EndData.instance.raceDone) {
                LapPanel.SetBool("End", true);
                PosPanel.SetBool("End", true);
                raceDone = true;
            }
            else {
                LapDisplayMaster lapDisplayMaster = player.players.lapDisplayMaster;
                place = lapDisplayMaster.GetPlayerPosition(player.playerNumber);
                lap = lapDisplayMaster.GetPlayerLap(player.playerNumber);
                SetPanels(lap, place);
            }  
        }        
	}

    private void SetPanels(int lap, int place)
    {
        LapPanel.SetInteger("Lap", lap);
        PosPanel.SetInteger("Position", place);
    }

}
