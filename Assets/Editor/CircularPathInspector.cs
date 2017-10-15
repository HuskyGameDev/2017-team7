using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircularPath))]
public class CircularPathInspector : Editor {
	private const int curveSteps = 10;

	private CircularPath path;
	private Transform handleTransform;
	private Quaternion handleRotation;
	
	private void OnSceneGUI() {
		path = target as CircularPath;
		handleTransform = path.transform;
		handleRotation = handleTransform.rotation;

		//For now, we'll show all points on the path
		for(int i = 0; i < path.points.Count ; i++){
			DisplayPoint(i);
		}
	}

	//Displays a point, then returns the transformed point
	private Vector3 DisplayPoint(int pIn){
		Vector3 point = handleTransform.TransformPoint(path.points[pIn].point);
		EditorGUI.BeginChangeCheck();
		point = Handles.DoPositionHandle(point, handleRotation);
		if(EditorGUI.EndChangeCheck()){
			Undo.RecordObject(path, "Move Point");
			EditorUtility.SetDirty(path);
			path.points[pIn] = new CubicBezierCurvePoint(new Vector2(point.x, point.y), path.points[pIn].controlPoint, path.points[pIn].type);
		}
		return point;
	}
}
