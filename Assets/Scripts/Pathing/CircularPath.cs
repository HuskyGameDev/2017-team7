﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPath : MonoBehaviour {
	public Collider2D finishLine;
	public CheckpointBehaviour defaultCheckpoint;
	[SerializeField]
	private List<CubicBezierCurve> curves;
	private List<CheckpointBehaviour> createdCheckpoints;
	private LapTracker tracker;
	//This is probably not how to do getters in C#, but this will work.
	void Start(){
		defaultCheckpoint = gameObject.GetComponentInChildren<CheckpointBehaviour>();
		tracker = gameObject.GetComponent<LapTracker>();
		int currentCheckpointNumber = 0;
		CheckpointBehaviour createdCheckpoint;
		createdCheckpoints = new List<CheckpointBehaviour>();
		
		if(curves == null || curves.Count == 0) return;
		//Use the default checkpoint as the first one.
		/*defaultCheckpoint.checkpointNumber = currentCheckpointNumber;
		defaultCheckpoint.trigger.offset = curves[0].p0Checkpoint.position;
		defaultCheckpoint.trigger.radius = curves[0].p0Checkpoint.radius;*/
		
		//Instantiate all checkpoint colliders
		currentCheckpointNumber++;
		for(;currentCheckpointNumber < curves.Count; currentCheckpointNumber++){
			createdCheckpoint = Instantiate(defaultCheckpoint, this.transform);

			createdCheckpoint.checkpointNumber = currentCheckpointNumber;
			createdCheckpoint.trigger.offset = curves[currentCheckpointNumber].p0Checkpoint.position;
			createdCheckpoint.trigger.radius = curves[currentCheckpointNumber].p0Checkpoint.radius;

			createdCheckpoints.Add(createdCheckpoint);
		}
	}
	public void SetCurves(List<CubicBezierCurve> curves_new){
		curves = curves_new;
	}

	public List<CubicBezierCurve> GetCurves(){
		return curves;
	}

	public CubicBezierCurve GetCurve(int i){
		return curves[i];
	}

	public CubicBezierCurve GetNextCurve(int i){
		return curves[(i + 1) % curves.Count];
	}

	public CubicBezierCurve GetPreviousCurve(int i){
		return curves[ i - 1 < 0 ? curves.Count - 1: i-1];
	}

	void OnDestroy(){
		//Destroy all the checkpoints we instantiated
		for(int i = 0;i<createdCheckpoints.Count;i++){
			Destroy(createdCheckpoints[i]);
		}
	}

	public void OnTriggerEnter2D(Collider2D other){
		Player p = other.gameObject.GetComponent<Player>();
		
		if(p == null) return;

		//Debug.Log("Entering checkpoint");
		tracker.PlayerCrossed(p.playerNumber, 0);
	}
}
