using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTracker : MonoBehaviour {
	private const int numPlayers = 4;
	private const int POINTS_PER_TABLE = 25;
	private const float MIN_PRECISION = 0.001f;
	private int[] curCounts;
	private int[] curPositionalCounts;
	private int[] positionalLaps;
	private int[] laps;
	private int totalCheckpoints;
	private List<CubicBezierCurve> curves;
	private Vector2[,] curveLookupTables;
	void Awake() {
		curves = GetComponent<CircularPath>().GetCurves();
		totalCheckpoints = curves.Count;
		curveLookupTables = new Vector2[curves.Count,POINTS_PER_TABLE];
		curCounts = new int[numPlayers];
		curPositionalCounts = new int[numPlayers];
		laps = new int[numPlayers];
		positionalLaps = new int[numPlayers];
		//Probably unnecessary, but just to be sure, zero these variables.
		for(int i = 0; i < numPlayers; i++){
			curCounts[i] = 0;
			laps[i] = 0;
			curPositionalCounts[i] = 0;
			positionalLaps[i] = 0;
		}

		//Generate lookup tables.
		int table = 0;
		foreach(CubicBezierCurve curve in curves){
			for(int i = 0;i<POINTS_PER_TABLE;i++){
				curveLookupTables[table, i] = curve.getPoint((float)i/(float)(POINTS_PER_TABLE-1));				
			}
			table++;
		}
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
			Debug.Log("Player " + player + " hit checkpoint" + checkpointNum + ".");
		}
	}

	public int[] GetPositions(Player[] players){
		int[] places = new int[players.Length];
		float[] tVals = new float[numPlayers];
		int curPlayer = 0;
		bool increasePositional;
		bool decreasePositional;
		bool continueLoop;
		//Calculate the projection onto the curve for each player.
		//I use the approach from https://pomax.github.io/bezierinfo/#projections
		foreach(Player p in players){
			continueLoop = true;
			increasePositional = false;
			decreasePositional = false;
			while(continueLoop){
				//Debug.Log("Count: " + curPositionalCounts[p.playerNumber - 1] + "\n");
				continueLoop = false;
				float minDist = (p.playerRB.position - curveLookupTables[curPositionalCounts[p.playerNumber - 1], 0]).magnitude;
				float deltaT = 1f/(float)(POINTS_PER_TABLE - 1);
				//Calculate closest in the lookup table
				for(int i = 1;i<POINTS_PER_TABLE;i++){
					float mag = (p.playerRB.position - curveLookupTables[curPositionalCounts[p.playerNumber - 1], i]).magnitude;
					if(mag < minDist){
						minDist = mag;
						tVals[p.playerNumber - 1] = (float)i/(float)(POINTS_PER_TABLE-1);
					}
				}
				//Do fine adjustments to find curve position
				while(deltaT > MIN_PRECISION){
					deltaT /= 2f;
					float tempT = tVals[p.playerNumber - 1] + deltaT > 1f ? 0 : tVals[p.playerNumber - 1] + deltaT;
					float mag = (p.playerRB.position - curves[curPositionalCounts[p.playerNumber - 1]].getPoint(tempT)).magnitude;
					if(mag < minDist){
						minDist = mag;
						tVals[p.playerNumber - 1] = tempT;
						continue;
					}

					tempT = tVals[p.playerNumber - 1] - deltaT < 0f ? 0f: tVals[p.playerNumber - 1] - deltaT;
					mag = (p.playerRB.position - curves[curPositionalCounts[p.playerNumber - 1]].getPoint(tempT)).magnitude;
					if(mag < minDist){
						minDist = mag;
						tVals[p.playerNumber - 1] = tempT;
						continue;
					}
				}
				//If the projection is 0 or 1, it's pretty possible that they're on the previous or next checkpoint
				//So we should treat them as if they are on the last checkpoint, and try again
				if(tVals[p.playerNumber - 1] == 1.0f){
					Debug.Log("Increasing position, current count: " + curPositionalCounts[p.playerNumber - 1] + ", tVal: " + tVals[p.playerNumber - 1] + ", Lap: " + positionalLaps[p.playerNumber - 1]);
					//We've been decreasing, now we want to increase. This means we are on the boundry, so we exit the loop.
					if(decreasePositional) break;

					increasePositional = true;
					
					if(curPositionalCounts[p.playerNumber - 1] + 1 == totalCheckpoints){
						curPositionalCounts[p.playerNumber - 1] = 0;
						positionalLaps[p.playerNumber - 1] += 1;
					}else{
						curPositionalCounts[p.playerNumber - 1] = curPositionalCounts[p.playerNumber - 1] + 1;
					}
					
					continueLoop = true;
				}

				if(tVals[p.playerNumber - 1] == 0.0f){
					Debug.Log("Decreasing position");
					if(increasePositional) break;

					decreasePositional = true;
					if(curPositionalCounts[p.playerNumber - 1] - 1 == -1){
						curPositionalCounts[p.playerNumber - 1] = totalCheckpoints - 1;
						positionalLaps[p.playerNumber - 1]-=1;
					}else{
					 curPositionalCounts[p.playerNumber - 1]  = curPositionalCounts[p.playerNumber - 1] - 1;
					}
					continueLoop = true;
				}

			}
			curPlayer++;
		}
		Debug.Log(curPositionalCounts[0] + ", " + curPositionalCounts[1] + ", " + curPositionalCounts[2] + ", " + curPositionalCounts[3]);
		Debug.Log(positionalLaps[0] + ", " + positionalLaps[1] + ", " + positionalLaps[2] + ", " + positionalLaps[3]);
		Debug.Log(tVals[0] + ", " + tVals[1] + ", " + tVals[2] + ", " + tVals[3]);
		Debug.Log("Determining Positions...");
		//I guarantee this is a dumb way to find positions.
		//There is almost certainly an O(nlgn) (or even O(n)) way to do this, but since 
		// 4 is the max for n, it's not a big deal, and this will work fine as O(n^2).
		curPlayer = 0;
		foreach(Player p1 in players){
			int numBefore = 0;
			foreach(Player p2 in players){
				if(p2 == p1) continue;
				if(positionalLaps[p2.playerNumber - 1] < positionalLaps[p1.playerNumber - 1]){
					numBefore++;
					continue;
				}
				if(positionalLaps[p2.playerNumber - 1] > positionalLaps[p1.playerNumber - 1]) continue;

				if(curPositionalCounts[p2.playerNumber - 1] < curPositionalCounts[p1.playerNumber - 1]){
					numBefore++;
					continue;
				}
				if(curPositionalCounts[p2.playerNumber - 1] > curPositionalCounts[p1.playerNumber - 1]) continue;

				if(tVals[p2.playerNumber - 1] < tVals[p1.playerNumber - 1]){
					numBefore++;
				}
			}
			places[curPlayer] = numPlayers - numBefore;
			curPlayer++;
		}
		return places;
	}
}
