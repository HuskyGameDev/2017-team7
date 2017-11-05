using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    private float turn;                         // Turning rotation
    private float forward;                      // Forward movement
    private float backward;
    private float A;
    private float B;
    private float X;
    private float Y;

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

    // Update all input values
    public void UpdateValues(float t, float f, float back, float a, float b,
                                float x, float y)
    {
        turn = -t;
        forward = f;
        backward = back;
        A = a;
        B = b;
        X = x;
        Y = y;
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

    // Return A button value
    public float GetA()
    {
        return A;
    }

    // Return B button value
    public float GetB()
    {
        return B;
    }

    // Return X button value
    public float GetX()
    {
        return X;
    }

    // Return Y button value
    public float GetY()
    {
        return Y;
    }
}
