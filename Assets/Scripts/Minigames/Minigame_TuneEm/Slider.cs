using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {

	private bool slide = false;
	private int direction = 1;
	public float speed;
	public float speedIncrement;
	public float redIncrement;
	private Transform redArea, greenArea, yellowSlider;

	// Use this for initialization
	void Start () {
		redArea = this.transform.Find("Slider_Red");
		greenArea = this.transform.Find("Slider_Green");
		yellowSlider = this.transform.Find("Slider_Bar");
		
		direction = Random.value >= 0.5	? 1 : -1;
		yellowSlider.localPosition = new Vector3(yellowSlider.localPosition.x,  
			Random.value * (greenArea.localScale.y - yellowSlider.localScale.y) - ((greenArea.localScale.y / 2) - (yellowSlider.localScale.y / 2)), 
			yellowSlider.localPosition.z);
	}
	
	public void BeginSlider(){
		slide = true;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(!slide) return;
		UpdateYellowSlider();
	}
	//Call to update the yellow slider movement;
	private void UpdateYellowSlider(){
		float newY = yellowSlider.localPosition.y + direction*speed;
		if( newY >= (greenArea.localScale.y / 2) - (yellowSlider.localScale.y / 2) || newY <= (yellowSlider.localScale.y / 2) - (greenArea.localScale.y / 2)){
			direction *= -1;
			newY = Mathf.Clamp(newY, 
				(yellowSlider.localScale.y / 2) - (greenArea.localScale.y / 2), 
				(greenArea.localScale.y / 2) - (yellowSlider.localScale.y / 2));
		}else{
			newY = yellowSlider.localPosition.y + direction*speed;
		}

		yellowSlider.localPosition = new Vector3(yellowSlider.localPosition.x,
			newY, 
			yellowSlider.localPosition.z);
	}
	//Call to pick a new red area for the slider
	private void UpdateRedSlider(){

	}
	//Call to check if the slider is in the red area 
	public bool IsInArea(){
		return false;
	}
}
