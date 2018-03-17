using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePumpItUpPlayer : MonoBehaviour {
    public int playerNum;

    int numOfPumps = 0;
    bool up = false;
    Controller controller;

	// Use this for initialization
	void Start () {
        controller = Inputs.GetController(playerNum);        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        if (controller != null)
        {
            if (up)
            {
                if (controller.GetLsYaxis() < -0.75)
                {
                    up = false;
                    numOfPumps++;
                    
                }
            }
            else
            {
                if (controller.GetLsYaxis() > 0.75)
                {
                    up = true;
                   
                }
            }
        }
	}




    public int GetNumOfPumps() { return numOfPumps; }
}
