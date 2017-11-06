using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircularPath))]
public class CircularPathInspector : Editor {
	private const int curveSteps = 50;
	private const float handleSize = 3.0f;
	private const float pickSize = 0.6f;
	private const float arrowLength = 40.0f;
	private CircularPath path;
	private Transform handleTransform;
	private Quaternion handleRotation;
	private List<CubicBezierCurve> curves;
	private int selected = -1;

	void OnEnable()
	{
		Tools.hidden = true;
	}
	
	void OnDisable()
	{
		Tools.hidden = false;
	}

	public override void OnInspectorGUI () {
		path = target as CircularPath;
		curves = path.GetCurves();
		path.transform.hideFlags = HideFlags.HideInInspector | HideFlags.NotEditable;
		handleTransform = path.transform;
		handleRotation = handleTransform.rotation;

		if(GUILayout.Button("Add Curve")){
			Undo.RecordObject(path, "Add Curve");
			EditorUtility.SetDirty(path);
			
			if(curves == null){
				path.SetCurves(new List<CubicBezierCurve>());
				curves = path.GetCurves();
			}

			selected = curves.Count;
			if(curves.Count > 0){
				curves.Add(new CubicBezierCurve());
				//TODO init control handles
				curves[curves.Count - 1].p0 = path.GetPreviousCurve(curves.Count - 1).p3;
				curves[curves.Count - 1].p3 = path.GetNextCurve(curves.Count - 1).p0;
			}else{
				curves.Add(new CubicBezierCurve());
			}
		}

		ShowCurveOptions();
	}
	private void OnSceneGUI() {
		path = target as CircularPath;
		curves = path.GetCurves();
		handleTransform = path.transform;
		handleRotation = handleTransform.rotation;
		
		if(curves == null) return;
		
		//For now, we'll show all points on the path
		for(int i = 0; i < curves.Count ; i++){
			if(i != selected){
				//Display one curve
				DisplayCurvePoints(i);
			}else{
				//Draw selected curve
				Vector3 point;
				
				//p0
				point = handleTransform.TransformPoint(curves[i].p0);
				EditorGUI.BeginChangeCheck();

				point = Handles.DoPositionHandle(point, handleRotation);
				
				Handles.DrawWireDisc(point, Vector3.forward, curves[i].p0Checkpoint.radius);

				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Move point");
					EditorUtility.SetDirty(path);
					curves[i].p0 = handleTransform.InverseTransformPoint(point);
					curves[i].p0Checkpoint.position = point;
					path.GetPreviousCurve(selected).p3 = handleTransform.InverseTransformPoint(point);
				}

				//p1
				point = handleTransform.TransformPoint(curves[i].p1);
				EditorGUI.BeginChangeCheck();

				point = Handles.DoPositionHandle(point, handleRotation);

				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Move point");
					EditorUtility.SetDirty(path);
					curves[i].p1 = handleTransform.InverseTransformPoint(point);
				}

				//p2
				point = handleTransform.TransformPoint(curves[i].p2);
				EditorGUI.BeginChangeCheck();

				point = Handles.DoPositionHandle(point, handleRotation);

				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Move point");
					EditorUtility.SetDirty(path);
					curves[i].p2 = handleTransform.InverseTransformPoint(point);
				}

				//p3
				point = handleTransform.TransformPoint(curves[i].p3);
				EditorGUI.BeginChangeCheck();

				point = Handles.DoPositionHandle(point, handleRotation);

				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Move point");
					EditorUtility.SetDirty(path);
					curves[i].p3 = handleTransform.InverseTransformPoint(point);
					path.GetNextCurve(selected).p0 = handleTransform.InverseTransformPoint(point);
					path.GetNextCurve(selected).p0Checkpoint.position = handleTransform.InverseTransformPoint(point);
				}
			}
			if(i == selected){
				Handles.color = Color.yellow;
			}else{
				Handles.color = Color.white;
			}
			float dt = 1.0f/curveSteps;
			Vector3 startPoint, endPoint;
			//Draw the actual curve
			for(int j = 0;j<curveSteps;j++){
				startPoint = handleTransform.TransformPoint(curves[i].getPoint(dt*j));
				endPoint = handleTransform.TransformPoint(curves[i].getPoint(dt*(j+1)));
				Handles.DrawLine(startPoint, endPoint);
			}

			//Draw an arrow midway through, indicating direction of the path
			Vector2 dir = curves[i].getDerivative(0.5f);
			dir *= 1.0f/dir.magnitude;
			Vector2 halfPoint = handleTransform.TransformPoint(curves[i].getPoint(0.5f));
			Handles.DrawLine(halfPoint, halfPoint + arrowLength*dir);
			//Vector2 arrowHalf;
			/* 
			float angle = (Mathf.PI/180.0f)*(Vector2.SignedAngle(Vector2.right, dir) - 45);
			
			arrowHalf = handleTransform.TransformPoint(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

			Handles.DrawLine(halfPoint, halfPoint + arrowLength*arrowHalf);

			angle = (Mathf.PI/180.0f)*(Vector2.SignedAngle(Vector2.right, dir) + 45);

			arrowHalf = handleTransform.TransformPoint(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

			Handles.DrawLine(halfPoint, halfPoint + arrowLength*arrowHalf);
			*/
		}

	}

	//Displays a point, then returns the transformed point
	private void DisplayCurvePoints(int cIn){
		CubicBezierCurve c = curves[cIn];
		Vector3 point, point2;
		//Don't redraw selected handles
		Handles.color = Color.cyan;
		point = handleTransform.TransformPoint(c.p0);

		Handles.DrawWireDisc(point, Vector3.forward, c.p0Checkpoint.radius);

		if(selected != cIn - 1 && (cIn - 1 == -1 && selected != curves.Count - 1)){
			if(Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap)){
				selected = cIn;
			}
		}

		point2 = handleTransform.TransformPoint(c.p1); 
		Handles.DrawLine(point, point2);

		Handles.color = Color.yellow;
		point = point2;
		if(Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap)){
			selected = cIn;
		}

		point = handleTransform.TransformPoint(c.p2);
		if(Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap)){
			selected = cIn;
		}
		Handles.color = Color.cyan;
		point2 = handleTransform.TransformPoint(c.p3);
		Handles.DrawLine(point, point2);

		//Don't redraw selected handles
		point = point2;
		if( (cIn + 1) % curves.Count != selected){

			if(Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap)){
				selected = cIn;
			}

		}
	}

	private void ShowCurveOptions(){
		
		if(selected >= 0){
			CubicBezierCurve c = path.GetCurve(selected);
			Vector2 point;
			//BezierPointType type;
			
			EditorGUI.BeginChangeCheck();
			//p0
			point = EditorGUILayout.Vector2Field("P0", c.p0);
			
			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				c.p0 = point;
				c.p0Checkpoint.position = point;
				path.GetPreviousCurve(selected).p3 = point;
			}
			/*
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			{
				type = (BezierPointType)EditorGUILayout.EnumPopup("Point 1 type:", c.p0Type);
			}
			EditorGUILayout.EndHorizontal();
			
			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				c.p0Type = type;
				if(type == BezierPointType.CHECKPOINT || type == BezierPointType.ADVANCE_CHECKPOINT){
					//c.checkpoint = new Checkpoint();
				}
			}
			*/
			EditorGUI.BeginChangeCheck();
			//p1
			point = EditorGUILayout.Vector2Field("P1", c.p1);
			
			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				c.p1 = point;
			}

			EditorGUI.BeginChangeCheck();
			//p2
			point = EditorGUILayout.Vector2Field("P2", c.p2);
			
			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				c.p2 = point;
			}

			EditorGUI.BeginChangeCheck();
			//p3
			point = EditorGUILayout.Vector2Field("P3", c.p3);

			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				c.p3 = point;
				path.GetNextCurve(selected).p0 = point;
				path.GetNextCurve(selected).p0Checkpoint.position = point;
			}
			/*
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			{
				CubicBezierCurve cNext = path.GetNextCurve(selected);
				type = (BezierPointType)EditorGUILayout.EnumPopup("Point 3 type:", cNext.p0Type);
			}
			EditorGUILayout.EndHorizontal();
			
			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				path.GetNextCurve(selected).p0Type = type;
				if(type == BezierPointType.CHECKPOINT || type == BezierPointType.ADVANCE_CHECKPOINT){
					//path.GetNextCurve(selected).checkpoint = new Checkpoint();
				}

			}
			*/
			float newRadius;
			//Vector2 newPosition;
			//if(c.p0Type == BezierPointType.CHECKPOINT || c.p0Type == BezierPointType.ADVANCE_CHECKPOINT){
				EditorGUI.BeginChangeCheck();
				newRadius = EditorGUILayout.FloatField("Trigger p0 radius", c.p0Checkpoint.radius);
				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Changed Collider");
					EditorUtility.SetDirty(path);
					c.p0Checkpoint.radius = newRadius;
				}
				/*
				EditorGUI.BeginChangeCheck();
				newPosition = EditorGUILayout.Vector2Field("Trigger position", c.p0Checkpoint.position);
				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Changed Collider");
					EditorUtility.SetDirty(path);
					c.p0Checkpoint.position = newPosition;
				}
				*/
			//}
			//If this is a checkpoint type, we oughtta show checkpoint options.
			//if(path.GetNextCurve(selected).p0Type == BezierPointType.CHECKPOINT || path.GetNextCurve(selected).p0Type == BezierPointType.ADVANCE_CHECKPOINT){
				EditorGUI.BeginChangeCheck();
				newRadius = EditorGUILayout.FloatField("Trigger p3 radius", path.GetNextCurve(selected).p0Checkpoint.radius);
				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Changed Collider");
					EditorUtility.SetDirty(path);
					path.GetNextCurve(selected).p0Checkpoint.radius = newRadius;
				}
				/*
				EditorGUI.BeginChangeCheck();
				newPosition = EditorGUILayout.Vector2Field("Trigger position", path.GetNextCurve(selected).p0Checkpoint.position);
				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Changed Collider");
					EditorUtility.SetDirty(path);
					path.GetNextCurve(selected).p0Checkpoint.position = newPosition;
				}
				*/
			//}

		}
	}

}
