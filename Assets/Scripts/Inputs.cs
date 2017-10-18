using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{

    private static Controller[] controllers;
    private int numPlayers = 2;

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
            float turning = Input.GetAxis("LeftStickX-" + player);
            float moveForward = Input.GetAxis("RightTrigger-" + player);
            float moveBackward = Input.GetAxis("LeftTrigger-" + player);
            controllers[player - 1].UpdateValues(turning, moveForward, moveBackward);
        }
    }

    // Returns Controller object for the given playerNum
    public static Controller GetController(int playerNum)
    {
        return controllers[playerNum - 1];
    }
}
