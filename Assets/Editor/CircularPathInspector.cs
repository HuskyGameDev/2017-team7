using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircularPath))]
public class CircularPathInspector : Editor {
	private const int curveSteps = 50;
	private const float handleSize = 0.5f;
	private const float pickSize = 0.6f;
	private CircularPath path;
	private Transform handleTransform;
	private Quaternion handleRotation;
	private List<CubicBezierCurve> curves;
	private int selected = -1;


	public override void OnInspectorGUI () {
		path = target as CircularPath;
		curves = path.GetCurves();
		handleTransform = path.transform;
		handleRotation = handleTransform.rotation;

		if(GUILayout.Button("Add Curve")){
			Undo.RecordObject(path, "Add Curve");
			EditorUtility.SetDirty(path);
			if(curves.Count > 0){
				curves.Add(new CubicBezierCurve());
				//TODO init control handles
				curves[curves.Count - 1].p0 = curves[curves.Count - 2 < 0 ? 0 : curves.Count - 2].p3;
				curves[curves.Count - 1].p3 = curves[0].p0;
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
		
		//For now, we'll show all points on the path
		for(int i = 0; i < curves.Count ; i++){
			if(i != selected){
				//Display one curve
				DisplayCurve(i);
			}else{
				//Draw selected curve
				Vector3 point;
				
				//p0
				point = handleTransform.TransformPoint(curves[i].p0);
				EditorGUI.BeginChangeCheck();

				point = Handles.DoPositionHandle(point, handleRotation);

				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Move point");
					EditorUtility.SetDirty(path);
					curves[i].p0 = point;
				}

				//p1
				point = handleTransform.TransformPoint(curves[i].p1);
				EditorGUI.BeginChangeCheck();

				point = Handles.DoPositionHandle(point, handleRotation);

				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Move point");
					EditorUtility.SetDirty(path);
					curves[i].p1 = point;
				}

				//p2
				point = handleTransform.TransformPoint(curves[i].p2);
				EditorGUI.BeginChangeCheck();

				point = Handles.DoPositionHandle(point, handleRotation);

				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Move point");
					EditorUtility.SetDirty(path);
					curves[i].p2 = point;
				}

				//p3
				point = handleTransform.TransformPoint(curves[i].p3);
				EditorGUI.BeginChangeCheck();

				point = Handles.DoPositionHandle(point, handleRotation);

				if(EditorGUI.EndChangeCheck()){
					Undo.RecordObject(path, "Move point");
					EditorUtility.SetDirty(path);
					curves[i].p3 = point;
				}
			}

			float dt = 1.0f/curveSteps;
			//Draw the actual curve
			for(int j = 0;j<curveSteps;j++){
				Handles.DrawLine(curves[i].getPoint(dt*j), curves[i].getPoint(dt*(j+1)));
			}

		}

	}

	//Displays a point, then returns the transformed point
	private void DisplayCurve(int cIn){
		CubicBezierCurve c = curves[cIn];
		Vector3 point, point2;
		//Don't redraw selected handles
		Handles.color = Color.cyan;
		point = handleTransform.TransformPoint(c.p0);
		if(selected != cIn - 1 || (cIn - 1 == -1 && selected != curves.Count)){
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

		point2 = handleTransform.TransformPoint(c.p3);
		Handles.DrawLine(point, point2);

		//Don't redraw selected handles
		Handles.color = Color.cyan;
		point = point2;
		if( (cIn + 1) % curves.Count == selected){
			
			if(Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap)){
				selected = cIn;
			}
		}
	}

	private void ShowCurveOptions(){
		
		if(selected >= 0){
			CubicBezierCurve c = curves[selected];
			Vector2 point;
			BezierPointType type;
			
			EditorGUI.BeginChangeCheck();
			//p0
			point = EditorGUILayout.Vector2Field("P0", c.p0);
			
			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				c.p0.x = point.x;
				c.p0.y = point.y;
			}

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
			}

			EditorGUI.BeginChangeCheck();
			//p1
			point = EditorGUILayout.Vector2Field("P1", c.p1);
			
			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				c.p1.x = point.x;
				c.p1.y = point.y;
			}

			EditorGUI.BeginChangeCheck();
			//p2
			point = EditorGUILayout.Vector2Field("P2", c.p2);
			
			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				c.p2.x = point.x;
				c.p2.y = point.y;
			}

			EditorGUI.BeginChangeCheck();
			//p3
			point = EditorGUILayout.Vector2Field("P3", c.p3);

			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				c.p3.x = point.x;
				c.p3.y = point.y;
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			{
				CubicBezierCurve cNext = curves[(selected + 1) % curves.Count];
				type = (BezierPointType)EditorGUILayout.EnumPopup("Point 3 type:", cNext.p0Type);
			}
			EditorGUILayout.EndHorizontal();

			if(EditorGUI.EndChangeCheck()){
				Undo.RecordObject(path, "Edited Curve");
				EditorUtility.SetDirty(path);
				curves[(selected + 1) % curves.Count].p0Type = type;
			}
		}
	}

}
