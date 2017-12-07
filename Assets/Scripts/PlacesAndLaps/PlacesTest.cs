using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlacesTest : MonoBehaviour {
	public LapTracker tracker;
	public Player[] players;
	public Text text;
	
	// Use this for initialization
	void Start () {
		text.text = "Places: []";
	}
	
	// Update is called once per frame
	void Update () {
		int[] places = tracker.GetPositions(players);
		text.text = "Places: [" + places[0] + "," + places[1] + "," + places[2] + "," + places[3] + "]";
	}
}
