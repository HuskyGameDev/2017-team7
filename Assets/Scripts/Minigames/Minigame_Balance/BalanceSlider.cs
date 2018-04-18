using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceSlider : MonoBehaviour {

	private enum SliderType {
		LEFT,
		RIGHT
	}
	public MinigameBalancePlayer player;

	[SerializeField]
	private SliderType type;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(GetNormalizedPosition());
		transform.position = new Vector3(transform.position.x, (GetNormalizedPosition() * 5.0f) - 2.5f);
	}

	private float GetNormalizedPosition(){
		switch(type){
			case SliderType.LEFT:
				return player.LeftTrigger;
			case SliderType.RIGHT:
				return player.RightTrigger;
			default:
				Debug.LogError("GetNormalizedPosition: Invalid type!");
				return 0f;
		}
	}
}
