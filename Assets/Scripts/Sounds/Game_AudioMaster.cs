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
        DoEvent("Play_Game_Music");
    }

    private void Start()
    {
        StartCoroutine(TestSound());
    }

    IEnumerator TestSound()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        Debug.Log("TESTSOUND");
        DoEvent("Play_Game_Music");
    }
}
