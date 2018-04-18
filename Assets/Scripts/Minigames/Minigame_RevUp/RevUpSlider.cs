using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevUpSlider : MonoBehaviour {
	public MinigameRevUp minigame;
	public float backgroundHeight;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(0, (minigame.Power * backgroundHeight)/2 - backgroundHeight/2);
		transform.localScale = new Vector3(1, minigame.Power * backgroundHeight);
	}
}
