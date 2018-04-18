using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_AudioMaster : AudioMaster {
    public void PlayMusic()
    {
        DoEvent("Play_Music");
    }

    public void EndMusic()
    {
        DoEvent("Stop_Music");
        DoEvent("Play_Whistle");
        //Play whistle and stop music
    }
}
