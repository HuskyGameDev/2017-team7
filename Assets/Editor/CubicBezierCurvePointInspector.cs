/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CubicBezierCurvePoint))]
public class CubicBezierCurvePointInspector : Editor {
	private CubicBezierCurvePoint point;
	private Transform handleTransform;
	private Quaternion handleRotation;
	private void OnSceneGUI() {
		point = target as CubicBezierCurvePoint;
		handleTransform = point.transform;
		handleRotation = handleTransform.rotation;

		Vector3 position = handleTransform.TransformPoint(point.point);

		EditorGUI.BeginChangeCheck();
		position = Handles.DoPositionHandle(position, handleRotation);

		if(EditorGUI.EndChangeCheck()){
			Undo.RecordObject(point, "Move Point");
			EditorUtility.SetDirty(point);
			point.point = handleTransform.InverseTransformPoint(position);
		}
	}
}
*/