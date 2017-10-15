using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType {
	CHECKPOINT = 0,
	ADVANCE_CHECKPOINT,
	NONE
}

public class PathSegment : MonoBehaviour {
	public CubicBezierCurve curve;
	public PointType startType;
	public PointType endType;

	static PathSegment Default(Vector2 start){
		PathSegment seg = new PathSegment();
		CubicBezierCurve curv = new CubicBezierCurve();
		curv.p0 = start;
		curv.p1 = start;
		curv.p2 = start + Vector2.right;
		curv.p3 = start + Vector2.right;

		seg.curve = curv;

		seg.startType = PointType.CHECKPOINT;
		seg.endType = PointType.CHECKPOINT;

		return seg;

	}
}
