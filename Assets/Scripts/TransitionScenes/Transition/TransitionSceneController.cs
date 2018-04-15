using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSceneController : MonoBehaviour {

    private enum STATES { }


    public void Start()
    {
        //Get 4 random powerups
        System.Random random = new System.Random();
        List<PowerupType> values = new List<PowerupType>((PowerupType[])Enum.GetValues(typeof(PowerupType)));
    }

    public void FixedUpdate()
    {
        
    }

}
