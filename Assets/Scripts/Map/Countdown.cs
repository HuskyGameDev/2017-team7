using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour {
    public Game_AudioMaster audioMaster;
    public Map mapEvents;

    private void Start()
    {
        if (mapEvents.disableCountdown)
            GetComponent<Animator>().SetTrigger("Skip");
    }

    public void Beep()
    {
        Debug.Log("BEEP");
        audioMaster.PlayBeep();
    }
    public void Go()
    {
        Debug.Log("GO");
        audioMaster.PlayRaceStart();
        mapEvents.StartRace();
    }
}
