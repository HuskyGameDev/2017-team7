﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEm_AudioMaster : Minigame_AudioMaster {
    public void PlayWrench()
    {
        DoEvent("Play_Wrench");
    }

    public void PlayWrenchFail()
    {
        DoEvent("Play_Wrench_Fail");
    }
}
