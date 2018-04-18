using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevUpBoundary : MonoBehaviour {
	public MinigameRevUp minigame;
	public float backgroundHeight;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(0, minigame.Target*backgroundHeight - backgroundHeight/2);		
	}
}
