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
    IEnumerator DelayGameMusic()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        DoEvent("Play_Game_Music");
    }
}
