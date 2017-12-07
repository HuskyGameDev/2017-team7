using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Text text = GetComponent<Text>();
		text.text = "Player " + EndData.completionOrder[0].ToString() + " Wins!";

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
