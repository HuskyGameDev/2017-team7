using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinigameBalancePlayer : MinigamePlayer
{
	public MinigameBalance minigame;
	/* These boundries are betwixt 0.0 and 1.0 */
	private float lowerBoundaryLeft;
	private float upperBoundaryLeft;
	private float lowerBoundaryRight;
	private float upperBoundaryRight;

	private uint curTicksInZone = 0;
	private uint maxTicksInZone = 0;

	/* get only properties: */
	public float LowerBoundaryLeft{
		get {
			return lowerBoundaryLeft;
		}
	}
	public float UpperBoundaryLeft{
		get {
			return upperBoundaryLeft;
		}
	}
	public float LowerBoundaryRight{
		get {
			return lowerBoundaryRight;
		}
	}
	public float UpperBoundaryRight{
		get {
			return upperBoundaryRight;
		}
	}

	public float LeftTrigger {
		get {
			if(controller != null) return controller.GetLT();
			return 0;
		}
	}

	public float RightTrigger {
		get {
			if(controller != null) return controller.GetRT();
			return 0;
		}
	}

	private IEnumerable inZoneCoroutine;
	private bool startedInZone = false;
    protected override void Init()
    {
		RandomizeBoundries();
    }

	protected override void ToReady()
    {

    }

    protected override void Tick()
	{
		if(InRightZone(RightTrigger) && InLeftZone(LeftTrigger)){
			
			if(!startedInZone){
				inZoneCoroutine = InZone();
				startedInZone = true;
			}
			
			curTicksInZone++;
			if(curTicksInZone > maxTicksInZone){
				maxTicksInZone = curTicksInZone;
			}

		}else{
			if(startedInZone){
				startedInZone = false;
				StopCoroutine(inZoneCoroutine.GetEnumerator());
			}
			curTicksInZone = 0;
		}
    }

	protected override void OnGameDone()
    {
		score += maxTicksInZone*Time.fixedDeltaTime / minigame.stayTime;
    }

	private void RandomizeBoundries(){
		float leftSize, rightSize;

		upperBoundaryRight = Random.Range(minigame.minBarSize, 1f);
		upperBoundaryLeft = Random.Range(minigame.minBarSize, 1f);

		rightSize = Random.Range(minigame.minBarSize, minigame.maxBarSize);
		leftSize = Random.Range(minigame.minBarSize, minigame.maxBarSize);

		lowerBoundaryRight = Mathf.Clamp01(upperBoundaryRight - rightSize);
		lowerBoundaryLeft = Mathf.Clamp01(upperBoundaryLeft - leftSize);
	}

	IEnumerable InZone(){
		yield return new WaitForSeconds(minigame.stayTime);
		NextZones();
	}

	private void NextZones(){
		score++;
		RandomizeBoundries();
		maxTicksInZone = 0;
		curTicksInZone = 0;
		//Possible triggers go here
	}

	private bool InLeftZone(float lt){
		return lt <= upperBoundaryLeft && lt >= lowerBoundaryLeft;
	}

	private bool InRightZone(float rt){
		return rt <= upperBoundaryRight && rt >= lowerBoundaryRight;
	}


}
