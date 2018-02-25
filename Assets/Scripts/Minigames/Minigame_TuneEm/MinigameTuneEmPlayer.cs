using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTuneEmPlayer : MonoBehaviour {
	public int playerNum;
	private Controller inputs;
	private bool aDown = false;
	private bool outOfTheGame = false;
	private int counts = 0;
	public Slider slider;
	// Use this for initialization
	void Start () {
		inputs = Inputs.GetController(playerNum + 1);
		
		slider.RegisterPlayerSlider(this.transform, this);
	}
	
	// Update is called once per frame
	void Update () {

		if(outOfTheGame) return;

		if(inputs.GetA()){
			if(!aDown){
				slider.RegisterHit(this);
			}
		}
				
	}

	public void Lose(){
		outOfTheGame = true;
	}
}
