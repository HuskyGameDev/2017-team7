using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameChargePlayer : MinigamePlayer {
    private int curSlice;
	private int maxSlice;

	public MinigameCharge minigame;

    protected override void ToReady()
    {
        
    }

    protected override void Init()
    {

    }

    protected override void OnGameDone()
    {
        score += GetPartialScore();
    }

    protected override void Tick()
    {
		//We need to invert the X axis, for whatever reason.
		CheckSlice(-controller.GetLsXaxis(), controller.GetLsYaxis());
		if(score >= minigame.maxCharges){
			minigame.End();
		}
    }

	//Check if the slice is new, increment it if it's the succesor slice, if it's now 0, call Crank 
	private void CheckSlice(float x, float y){
		float curAngle = NormalizeAngle(Mathf.Atan2(y, x));
		//Debug.Log("X: " + x + "Y: " + y);
		//Debug.Log("Cur angle: " + curAngle);
		float curMagnitude = Mathf.Sqrt(x*x + y*y);
		float angleBetweenSlices = 2*Mathf.PI / minigame.numSlices;
		float nextSliceMidAngle = NormalizeAngle((curSlice + 1) * angleBetweenSlices);

		if(Mathf.Abs(Mathf.DeltaAngle(curAngle, nextSliceMidAngle)) < (angleBetweenSlices / 2) &&
			curMagnitude >= minigame.minMagnitude){
			//Increment slice. Should also (possibly)trigger the players arm movement later.
			curSlice++;

			if(curSlice == minigame.numSlices){
				curSlice = 0;
				Crank();
			}
		
		}
	}

	//One rotation done, crank
	private void Crank(){
		score++;
		Debug.Log("Player " + playerNum + " got a crank. " + score );
	}

	private float GetPartialScore(){
		return curSlice/minigame.numSlices;
	}

	//Takes in an angle, and spits it out, normalized.
	//Normalized, in this case, means to place it on the interval of
	//[0, 2*Mathf.PI)
	private static float NormalizeAngle(float x){
		if(x >= 2*Mathf.PI){
			//This essentially performs a modulus operation, except wiht
			//floats.
			int num = Mathf.FloorToInt(x / (2*Mathf.PI));
			return x - (num*2*Mathf.PI);
		}

		if(x < 0){
			int num = Mathf.FloorToInt(x / (-2*Mathf.PI));
			float neg = x + (num*2*Mathf.PI);
			return x + 2*Mathf.PI;
		}

		return x;
	}

}
