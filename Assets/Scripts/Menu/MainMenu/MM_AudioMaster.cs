using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_AudioMaster : MonoBehaviour {
    public void PlayToggle()
    {
        PlaySound("Play_Toggle");
    }
    public void PlaySelect()
    {
        PlaySound("Play_Select");
    }


    private void PlaySound(string soundName)
    {
        AkSoundEngine.PostEvent(soundName, gameObject);
    }

}
