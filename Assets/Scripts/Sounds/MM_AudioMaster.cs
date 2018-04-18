using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_AudioMaster : AudioMaster {
    public void PlayToggle()
    {
        DoEvent("Play_Toggle");
    }
    public void PlaySelect()
    {
        DoEvent("Play_Select");
    }

    public void PlayMusic()
    {
        DoEvent("Play_MM_Music");
    }

    public void ChangeToDrums()
    {
        DoEvent("Change_To_Drums");
    }
    public void ChangeToNormal()
    {
        DoEvent("Change_To_Normal");
    }
}
