using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinigameRevUp : Minigame
{
	private enum RevUpState {
		IDLE,
		REVVING,
		DRIVING,
		SELECTING
	}

	public float velocityDistanceProportionality;
	public float width = 0.05f;
	public float borderMin = 0.5f;
	public float borderMax = 0.8f;
	public float rtThresh = 0.1f;

	private int currentPlayer = 1;
	//Power between 0 and 1
	private float power = 0;
	//Direction is either 1 or -1, depending on if
	//power is increasing or decreasing, respectively.
	private int direction = 1;

	private float target;
	
	private RevUpState state = RevUpState.IDLE;
	//Properties:
	public float Power {
		get {
			return power;
		}
	}

	public float Target{
		get {
			return target;
		}
	}

	public int CurrentPlayer {
		get {
			return currentPlayer;
		}
	}

	void Start(){
		RandomizeTarget();
	}

	protected override void InitMinigame()
    {
		currentPlayer = players.First(x => x.isActiveAndEnabled).playerNum;
    }

    public override void Tick()
    {
		if(currentPlayer == -1) return;

		switch(state){
			case RevUpState.REVVING:
				if(Inputs.GetController(currentPlayer).GetRT() < rtThresh){
					Debug.Log("Locked in at " + power);
					Debug.Log("Score = " + GetScore(target, power));
					//Player let go of RT, power is locked in!
					((MinigameRevUpPlayer)players[currentPlayer]).Score = GetScore(target, power);
					state = RevUpState.DRIVING;
					break;
				}
				//Calculating power, and the next direction
				power = Mathf.Clamp01(power + direction*GetSpeed());
				if(Mathf.Approximately(power, 1f)){
					direction = -1;
				}else if(Mathf.Approximately(power, 0)){
					direction = 1;
				}


				break;
			case RevUpState.DRIVING:
				//This state is where we will be while we are animating the
				//"driving" or skidding or whatever
				//Just skips this state for now
				state = RevUpState.SELECTING;
				break;
			case RevUpState.SELECTING:
				//State for selecting next player. maybe there's an animation here,
				//maybe not.
				//Right now, it just selects the next player
				currentPlayer = GetNextPlayer(currentPlayer);
				Debug.Log("Selecting player " + currentPlayer);
				state = RevUpState.IDLE;
				break;
			case RevUpState.IDLE:
				power = 0;
				//Waiting for player to start revving
				if(Inputs.GetController(currentPlayer).GetRT() >= rtThresh){
					Debug.Log("Revving up!");
					//Player let go of RT, power is locked in!
					state = RevUpState.REVVING;
				}
				break;
		}
    }

	public override bool HasEnded()
    {
		return currentPlayer == -1;
    }

	private float GetSpeed(){
		return velocityDistanceProportionality * (power + 0.1f);
	}

	private void RandomizeTarget(){
		target = Random.Range(borderMin, borderMax);
	}

	private int GetNextPlayer(int currentPlayer){
		if(players.Where(x => x.isActiveAndEnabled && x.playerNum > currentPlayer).Count() <= 0) return -1;
		return players.First(x => x.isActiveAndEnabled && x.playerNum > currentPlayer).playerNum;
	}

	private float GetScore(float target, float power){
		return 1f - (Mathf.Abs(target - power));
	}
}
