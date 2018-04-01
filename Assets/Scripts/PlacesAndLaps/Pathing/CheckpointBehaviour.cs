using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointBehaviour : MonoBehaviour {
	public CircleCollider2D trigger;
	public int checkpointNumber {get; set;}
	[SerializeField]
	private LapTracker lapTracker;

	void Start(){
		lapTracker = gameObject.GetComponentInParent<LapTracker>();
		//Debug.Log(lapTracker);

	}

	public void OnTriggerEnter2D(Collider2D other){
		if(other.tag != "PlayerWallCollider") return;

		Player p = other.GetComponentInParent<Player>();
		if(p == null) return;

		lapTracker.PlayerCrossed(p.playerNumber, checkpointNumber);
	}
}
