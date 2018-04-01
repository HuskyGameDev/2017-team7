using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameCharge : Minigame {

	public int numSlices;
	public float maxTime;
	public int maxCharges;
	public float minMagnitude;
    private bool isDone = false;

    IEnumerator timer;
    protected override void InitMinigame()
    {
        timer = StartTimer();
        StartCoroutine(timer);
    }

    public override void Tick()
    {

    }

    public override bool HasEnded()
    {
    	return isDone;
    }

    IEnumerator StartTimer(){
        yield return new WaitForSecondsRealtime(maxTime);
        End();
    }

    public void End(){
        StopCoroutine(timer);
        isDone = true;
    }

}
