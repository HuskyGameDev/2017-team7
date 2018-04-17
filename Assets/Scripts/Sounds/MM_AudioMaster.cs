using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_AudioMaster : AudioMaster {
    public void PlayToggle()
    {
        PlaySound("Play_Toggle");
    }
    public void PlaySelect()
    {
        PlaySound("Play_Select");
    }

    public void PlayMusic()
    {
        PlaySound("Play_MM_Music");
    }

    public void ChangeToDrums()
    {
        PlaySound("Change_To_Drums");
    }
    public void ChangeToNormal()
    {
        PlaySound("Change_To_Normal");
    }
}
