using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour {
	public CircleCollider2D trigger;
	public int checkpointNumber {get; set;}

	public void OnTriggerEnter(Collider2D other){
		//TODO mark this as met for a particular player
		Debug.Log("Hit the trigger!");
	}
}
