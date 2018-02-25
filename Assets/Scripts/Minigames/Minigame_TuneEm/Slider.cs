using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {

	private struct PlayerSlider {
		public Transform transform;
		public bool active;
		public MinigameTuneEmPlayer player;
	}

	private bool slide = false;
	private int direction = 1;
	public float speed;
	public float speedIncrement;
	public float redIncrement;
	public float redLength;
	private Transform redArea, greenArea;
	private List<PlayerSlider> playerSliders;

	// Use this for initialization
	void Awake () {
		redArea = this.transform.Find("Slider_Red");
		greenArea = this.transform.Find("Slider_Green");
		playerSliders = new List<PlayerSlider>();
		//Select a random direction and yellow slider position to begin with.
		//direction = Random.value >= 0.5	? 1 : -1;
		/*
		yellowSlider.localPosition = new Vector3(yellowSlider.localPosition.x,  
			Random.value * (greenArea.localScale.y - yellowSlider.localScale.y) - ((greenArea.localScale.y / 2) - (yellowSlider.localScale.y / 2)), 
			yellowSlider.localPosition.z);
		*/
		RandomRedSlider();
	}
	
	public void RegisterPlayerSlider(Transform slider, MinigameTuneEmPlayer p){
		PlayerSlider pSlider = new PlayerSlider();
		pSlider.transform = slider;
		pSlider.active = true;
		pSlider.player = p;
		playerSliders.Add(pSlider);
		Debug.Log("Player " + (p.playerNum + 1) + "Registered.");
	}

	public void BeginSlider(){
		slide = true;
		ResetSliders();
	}

	void FixedUpdate () {
		if(!slide) {
			ResetSliders();
			return;
		}
		UpdateSliders();
	}
	//Call to update the yellow slider movement;
	private void UpdateSliders(){
		float newY;
		for(int i = 0; i < playerSliders.Count; i++){
			PlayerSlider slider = playerSliders[i];
			Debug.Log("Player " + (slider.player.playerNum + 1) + ": " + slider.active);
			if(!slider.active) continue;

			newY = slider.transform.localPosition.y + direction*speed;
			if( newY >= (greenArea.localScale.y / 2) - (slider.transform.localScale.y / 2)){
				//TODO insert code that eliminates player from game
				slider.active = false;
				slider.player.Lose();
			} else {
				newY = slider.transform.localPosition.y + direction*speed;
			}

			slider.transform.localPosition = new Vector3(slider.transform.localPosition.x,
				newY, 
				slider.transform.localPosition.z);
			playerSliders[i] = slider;
		}
	}

	public void ResetSliders(){
		for(int i = 0; i < playerSliders.Count; i++){
			PlayerSlider slider = playerSliders[i];
			slider.active = true;
			slider.transform.localPosition = new Vector3(slider.transform.localPosition.x, 
				greenArea.localPosition.y - greenArea.localScale.y/2, 
				slider.transform.localPosition.z);
			Debug.Log("Player " + slider.player.playerNum + 1 +":");
			Debug.Log(greenArea.localPosition.y - greenArea.localScale.y/2);
			Debug.Log(slider.transform.localPosition.y);
		}
	}
	//Call to pick a new red area for the slider
	private void UpdateRedSlider(){
		redLength -= redIncrement;
		RandomRedSlider();
	}

	private void RandomRedSlider(){
		float ypos = Random.value * (greenArea.localScale.y - redLength) - (greenArea.localScale.y/2 - redLength/2);
		Debug.Log("Ypos: " + ypos);
		redArea.localPosition = new Vector3(redArea.localPosition.x, ypos, redArea.localPosition.z);
		redArea.localScale = new Vector3(redArea.localScale.x, redLength, redArea.localScale.z);
	}
	public void RegisterHit(MinigameTuneEmPlayer p){
		//speed += speedIncrement;
		//UpdateRedSlider();
		for(int i = 0; i < playerSliders.Count; i++){
			PlayerSlider slider = playerSliders[i];
			if(slider.player == p){
				Debug.Log("Deactivating player " + (p.playerNum + 1));
				slider.active = false;

				if(!IsInArea(slider.transform)){
					slider.player.Lose();
				}
				//slider is not a reference, it's a copy. 
				//So playerSliders[i] must be overwritten with slider.
				playerSliders[i] = slider;
			}
		}
	}
	//Call to check if the slider is in the red area 
	private bool IsInArea(Transform trans){
		float ypos = trans.localPosition.y;
		float ymax = redArea.localPosition.y + redArea.localScale.y / 2;
		float ymin = redArea.localPosition.y - redArea.localScale.y / 2;
		
		if(ypos <= ymax && ypos >= ymin){
			return true;
		}

		Debug.Log("Failed - ypos: " + ypos + ", ymax: " + ymax + ", ymin: " + ymin );	
		return false;
	}
}
