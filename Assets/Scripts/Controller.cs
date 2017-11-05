using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller {

    private float turn;                         // Turning rotation
    private float forward;                      // Forward movement
    private float backward;

    // Thumbsticks
    private float lsXaxis;                      // Left stick X axis
    private float lsYaxis;                      // Left stick Y axis
    private float lsClick;                      // Left stick click
    private float rsXaxis;                      // Right stick X axis
    private float rsYaxis;                      // Right stick Y axis
    private float rsClick;                      // Right stick click

    // Triggers and bumpers
    private float LT;                           // Left trigger
    private float RT;                           // Right trigger
    private float LB;                           // Left bumper
    private float RB;                           // Right bumper

    // Buttons
    private float A;                            // A button
    private float B;                            // B button
    private float X;                            // X button
    private float Y;                            // Y button
    private float back;                         // back button
    private float start;                        // start button

    // D-Pad
    private float dPadXaxis;                    // D-Pad X axis
    private float dPadYaxis;                    // D-Pad Y axis


    // Update all input values
    public void UpdateValues(float t, float f, float b)
    {
        lsXaxis = -t;
        RT = f;
        LT = b;
    }

    // Update all input values
    public void UpdateValues(float lsX, float lsY, float lsC, float rsX, float rsY, float rsC,
                                float lt, float rt, float lb, float rb,
                                float a, float b, float x, float y, float ba, float st,
                                float dpX, float dpY)
    {
        lsXaxis = lsX;
        lsYaxis = lsY;
        lsClick = lsC;
        rsXaxis = rsX;
        rsYaxis = rsY;
        rsClick = rsC;

        LT = lt;
        RT = rt;
        LB = lb;
        RB = rb;

        A = a;
        B = b;
        X = x;
        Y = y;
        back = ba;
        start = st;

        dPadXaxis = dpX;
        dPadYaxis = dpY;
    }


    //--------------------------------------------------------------------------------------------------------
    // Race gets

    // Return turning value
    public float GetTurn()
    {
        return GetLsXaxis();
    }

    // Return speed value
    public float GetSpeed()
    {
        return GetRT() - GetLT();
    }


    //--------------------------------------------------------------------------------------------------------
    // Thumbstick gets

    // Return left stick X axis value
    public float GetLsXaxis()
    {
        return lsXaxis;
    }

    // Return left stick Y axis value
    public float GetLsYaxis()
    {
        return lsYaxis;
    }

    // Return left stick click value
    public float GetLsClick()
    {
        return lsClick;
    }

    // Return right stick X axis value
    public float GetRsXaxis()
    {
        return rsXaxis;
    }

    // Return right stick Y axis value
    public float GetRsYaxis()
    {
        return rsYaxis;
    }

    // Return right stick click value
    public float GetRsClick()
    {
        return rsClick;
    }


    //--------------------------------------------------------------------------------------------------------
    // Trigger and bumper gets

    // Return left trigger value
    public float GetLT()
    {
        return LT;
    }

    // Return right trigger value
    public float GetRT()
    {
        return RT;
    }

    // Return left bumper value
    public float GetLB()
    {
        return LB;
    }

    // Return right bumper value
    public float GetRB()
    {
        return RB;
    }


    //--------------------------------------------------------------------------------------------------------
    // Button gets

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

    // Return back button value
    public float GetBack()
    {
        return back;
    }

    // Return start button value
    public float GetStart()
    {
        return start;
    }


    //--------------------------------------------------------------------------------------------------------
    // D-Pad gets

    // Return D-Pad X axis value
    public float GetDPadXaxis()
    {
        return dPadXaxis;
    }

    // Return D-Pad Y axis value
    public float GetDPadYaxis()
    {
        return dPadYaxis;
    }
}
