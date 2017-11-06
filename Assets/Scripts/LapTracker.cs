using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTracker : MonoBehaviour {
	private const int numPlayers = 4;
	private int[] curCounts;
	private int[] laps;
	private int totalCheckpoints;
	void Awake() {
		curCounts = new int[numPlayers];
		laps = new int[numPlayers];
		for(int i = 0; i < numPlayers; i++){
			curCounts[i] = 0;
			laps[i] = 0;
		}
	}
	
	public void SetTotalCheckpoints(int total) {
		totalCheckpoints = total;
	}

	public void PlayerCrossed(int player, int checkpointNum) {
		//Debug.Log("Player " + player + " Hit checkpoint " + checkpointNum);
		//Debug.Log("Count: " + curCounts[player-1]);
		if(checkpointNum == 0 && curCounts[player-1] == totalCheckpoints - 1){
			laps[player-1]++;
			curCounts[player-1] = 0;
			Debug.Log("Player " + player + " crossed the finish line.");
			return;
		}
		if(checkpointNum - 1 == curCounts[player-1]){
			curCounts[player-1] = checkpointNum;
			//Debug.Log("Player " + player + " hit a checkpoint.");
		}
	}
}
