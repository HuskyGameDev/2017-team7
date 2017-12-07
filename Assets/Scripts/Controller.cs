using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller {

    // Thumbsticks
    private float lsXaxis;                      // Left stick X axis
    private float lsYaxis;                      // Left stick Y axis
    private bool lsClick;                      // Left stick click
    private float rsXaxis;                      // Right stick X axis
    private float rsYaxis;                      // Right stick Y axis
    private bool rsClick;                      // Right stick click

    // Triggers and bumpers
    private float LT;                           // Left trigger
    private float RT;                           // Right trigger
    private bool LB;                           // Left bumper
    private bool RB;                           // Right bumper

    // Buttons
    private bool A;                            // A button
    private bool B;                            // B button
    private bool X;                            // X button
    private bool Y;                            // Y button
    private bool back;                         // back button
    private bool start;                        // start button

    // D-Pad
    private float dPadXaxis;                    // D-Pad X axis
    private float dPadYaxis;                    // D-Pad Y axis


    // Update all input values
    public void UpdateValues(float lsX, bool lsC, bool rsC,
                                float lt, float rt, bool lb, bool rb,
                                bool a, bool b, bool x, bool y, bool ba, bool st)
    {
        lsXaxis = -lsX;
        lsClick = lsC;
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
    }

    // Update all input values
    public void UpdateValues(float lsX, float lsY, bool lsC, float rsX, float rsY, bool rsC,
                                float lt, float rt, bool lb, bool rb,
                                bool a, bool b, bool x, bool y, bool ba, bool st,
                                float dpX, float dpY)
    {
        lsXaxis = -lsX;
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
    public bool GetLsClick()
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
    public bool GetRsClick()
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
    public bool GetLB()
    {
        return LB;
    }

    // Return right bumper value
    public bool GetRB()
    {
        return RB;
    }


    //--------------------------------------------------------------------------------------------------------
    // Button gets

    // Return A button value
    public bool GetA()
    {
        return A;
    }

    // Return B button value
    public bool GetB()
    {
        return B;
    }

    // Return X button value
    public bool GetX()
    {
        return X;
    }

    // Return Y button value
    public bool GetY()
    {
        return Y;
    }

    // Return back button value
    public bool GetBack()
    {
        return back;
    }

    // Return start button value
    public bool GetStart()
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
