using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEmBounds : MonoBehaviour {
    public TuneEm tuneEm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float[] bounds = tuneEm.GetBounds();
        float boundLength = bounds[1] - bounds[0];
        float y = (bounds[0] + (boundLength) / 2) * 5;
        //Debug.Log(y);
        transform.position = new Vector3(0, y);
        transform.localScale = new Vector3(1, boundLength * 5);
	}
}
