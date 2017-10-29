using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPath : MonoBehaviour {
	private List<CubicBezierCurve> curves = new List<CubicBezierCurve>();

	//This is probably not how to do getters in C#, but this will work.
	public List<CubicBezierCurve> GetCurves(){
		return curves;
	}
}
