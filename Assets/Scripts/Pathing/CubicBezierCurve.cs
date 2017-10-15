using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicBezierCurve : MonoBehaviour {
	public Vector2 p0, p1, p2, p3;

	public Vector2 getPoint(float t){
		return ((1.0f-t)*(1.0f-t)*(1.0f-t)*p0) + (3.0f*(1.0f-t)*(1.0f-t)*t*p1) + (3.0f*(1.0f-t)*t*t*p2) + (t*t*t*p3);
	}
}
