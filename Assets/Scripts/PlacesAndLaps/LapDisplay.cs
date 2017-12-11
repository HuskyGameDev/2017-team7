using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapDisplay : MonoBehaviour {
	public int player;
	//TODO change sprites instead of text.
	private Text text;
	private LapDisplayMaster master;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		master = GetComponentInParent<LapDisplayMaster>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Lap: " + master.GetPlayerLap(player).ToString() + "/" + master.lapTracker.maxLaps;
	}
}
