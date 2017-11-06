using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BezierPointType {
	CHECKPOINT = 0,
	ADVANCE_CHECKPOINT,
	NONE
}
[System.Serializable]
public class CubicBezierCurve{
	public Vector2 p0, p1, p2, p3;
	public BezierPointType p0Type = BezierPointType.NONE;
	public Checkpoint p0Checkpoint;
	public CubicBezierCurve(){
		p0 = new Vector2(0.0f, 0.0f);
		p1 = new Vector2(0.0f, 0.0f);
		p2 = new Vector2(0.0f, 0.0f);
		p3 = new Vector2(0.0f, 0.0f);
	}
	public Vector2 getPoint(float t){
		return ((1.0f-t)*(1.0f-t)*(1.0f-t)*p0) + (3.0f*(1.0f-t)*(1.0f-t)*t*p1) + (3.0f*(1.0f-t)*t*t*p2) + (t*t*t*p3);
	}
	public Vector2 getDerivative(float t){
		return 3.0f*(1-t)*(1-t)*(p1-p0) + 6.0f*(1-t)*t*(p2-p1) + 3.0f*t*t*(p3-p2);
	}
}
