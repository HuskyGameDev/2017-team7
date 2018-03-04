using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LapDisplayMaster : MonoBehaviour {
	private int[] positions;
	public LapTracker lapTracker;
    public Players players;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Maybe do this in on Tick instead?
		//positions = lapTracker.GetPositions(players);
	}

	void FixedUpdate () {
		Player[] playerArray = (Player[])players.players.Clone();
		playerArray.Where(x => x.isActiveAndEnabled);
		positions = lapTracker.GetPositions(players.players);
	}

	public int GetPlayerPosition(int playerNumber){
		if(positions == null) return 4;
		for(int i = 0;i < players.players.Length; i++){
			if(players.players[i].playerNumber == playerNumber){
				return positions[i];
			}
		}
		return -1;
	}

	public int GetPlayerLap(int playerNumber){
		return lapTracker.getPlayerLap(playerNumber) + 1;
	}
}
