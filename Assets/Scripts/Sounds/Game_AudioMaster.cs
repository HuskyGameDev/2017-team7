using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_AudioMaster : AudioMaster {

    public void PlayBeep()
    {
        Debug.Log("PLAY BEEP");
        DoEvent("Play_Beep");
    }
    public void PlayRaceStart()
    {
        DoEvent("Play_Go");
        StartCoroutine(DelayGameMusic());
    }
    public void EndRaceMusic()
    {
        //TODO: create end race music event
        //DoEvent("Play_Go")
        //DoEvent("Play_Whistle")
    }
    IEnumerator DelayGameMusic()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        DoEvent("Play_Game_Music");
    }
}
