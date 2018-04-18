using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBound : MonoBehaviour {

	private enum BoundType {
		LEFT,
		RIGHT
	}

	public MinigameBalancePlayer player;

	[SerializeField]
	private BoundType type;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float max, min, length, y;

		max = GetNormalizedMax();
		min = GetNormalizedMin();
		length = max - min;
		y = ((min + (length/2)) * 5) - 2.5f;

		transform.position = new Vector3(transform.position.x, y);
		transform.localScale = new Vector3(1, length * 5);
	}

	private float GetNormalizedMax(){
		switch(type){
			case BoundType.LEFT:
				return player.UpperBoundaryLeft;
			case BoundType.RIGHT:
				return player.UpperBoundaryRight;
			default:
				return 1f;
		}
	}

	private float GetNormalizedMin(){
		switch(type){
			case BoundType.LEFT:
				return player.LowerBoundaryLeft;
			case BoundType.RIGHT:
				return player.LowerBoundaryRight;
			default:
				return 0f;
		}
	}
}
