using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    private float turn;                         // Turning rotation
    private float forward;                      // Forward movement
    private float backward;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Update all input values
    public void UpdateValues(float t, float f, float b)
    {
        turn = -t;
        forward = f;
        backward = b;
    }

    // Return turning value
    public float GetTurn()
    {
        return turn;
    }

    // Return speed value
    public float GetSpeed()
    {
        return forward - backward;
    }
}
