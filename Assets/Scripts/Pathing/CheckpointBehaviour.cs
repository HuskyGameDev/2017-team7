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
		Player p = other.gameObject.GetComponent<Player>();
		
		if(p == null) return;

		//Debug.Log("Entering checkpoint");
		lapTracker.PlayerCrossed(p.playerNumber, checkpointNumber);
	}
}
