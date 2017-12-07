using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{

    private static Controller[] controllers;
    private int numPlayers = 4;

    // Use this for initialization
    void Awake()
    {
        controllers = new Controller[numPlayers];

        for (int i = 0; i < numPlayers; i++)
        {
            controllers[i] = new Controller();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int player = 1; player <= numPlayers; player++)
        {
            float lsX = Input.GetAxis("LeftStickX-" + player);

            bool lsClick = Input.GetKey("joystick " + player + " button 8");
            bool rsClick = Input.GetKey("joystick " + player + " button 9");

            float rt = Input.GetAxis("RightTrigger-" + player);
            float lt = Input.GetAxis("LeftTrigger-" + player);
            bool lb = Input.GetKey("joystick " + player + " button 4");
            bool rb = Input.GetKey("joystick " + player + " button 5");

            bool aButton = Input.GetKey("joystick " + player + " button 0");
            bool bButton = Input.GetKey("joystick " + player + " button 1");
            bool xButton = Input.GetKey("joystick " + player + " button 2");
            bool yButton = Input.GetKey("joystick " + player + " button 3");
            bool back = Input.GetKey("joystick " + player + " button 6");
            bool start = Input.GetKey("joystick " + player + " button 7");

            controllers[player - 1].UpdateValues(lsX, lsClick, rsClick, lt, rt, lb, rb,
                                aButton, bButton, xButton, yButton, back, start);
        }
    }

    // Returns Controller object for the given playerNum
    public static Controller GetController(int playerNum)
    {
        return controllers[playerNum - 1];
    }
}
