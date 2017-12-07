using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapDisplayMaster : MonoBehaviour {
	public Player[] players;
	private int[] positions;
	public LapTracker lapTracker;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Maybe do this in onTick() instead?
		//positions = lapTracker.GetPositions(players);
	}

	void FixedUpdate () {
		positions = lapTracker.GetPositions(players);
	}

	public int GetPlayerPosition(int playerNumber){
		if(positions == null) return 4;
		for(int i = 0;i < players.Length; i++){
			if(players[i].playerNumber == playerNumber){
				return positions[i];
			}
		}
		return -1;
	}

	public int GetPlayerLap(int playerNumber){
		return lapTracker.getPlayerLap(playerNumber) + 1;
	}
}
