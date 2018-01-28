using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimer : MonoBehaviour {
    public int player;
    //TODO change sprites instead of text.
    private Text text;
    private LapDisplayMaster master;
    // Use this for initialization
    float timer;
    public int lap;
    int numLaps = 0;
    string lapTime = "";
    float startTime;

    bool switched = false;


    void Start()
    {
        text = GetComponent<Text>();
        master = GetComponentInParent<LapDisplayMaster>();
        //public float timer[maste]
        numLaps = master.lapTracker.maxLaps;

    }

    // Update is called once per frame
    void Update()
    {
        if (!switched)
        {
            startTime = Time.time;
            if (lap == master.GetPlayerLap(player))
            {
                switched = true;
            }
        }
        if (master.GetPlayerLap(player) == lap)
        {
            timer = Time.time - startTime;
        }


        text.text = "Lap " + lap + ": " + timer.ToString("0.00");


        //Debug.Log(currentLap);
    
    }
}
