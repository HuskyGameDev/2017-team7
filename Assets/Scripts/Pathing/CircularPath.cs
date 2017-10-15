using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BezierPointType {
	CHECKPOINT = 0,
	ADVANCE_CHECKPOINT,
	NONE
}

public struct CubicBezierCurvePoint {
	public Vector2 point;
	public bool controlPoint;
	public BezierPointType type;

	public CubicBezierCurvePoint(Vector2 p, bool cp, BezierPointType t){
		point = p;
		controlPoint = cp;
		type = t;
	}
}


public class CircularPath : MonoBehaviour {
	public List<CubicBezierCurvePoint> points;
}
