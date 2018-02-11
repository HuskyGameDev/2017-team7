using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Checkpoint {
	public Vector2 position;
	public float radius;

	public Checkpoint(Vector2 posin, float radius){
		position = posin;
		this.radius = radius;
	}
}
