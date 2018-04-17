using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour {
    protected void PlaySound(string soundName)
    {
        AkSoundEngine.PostEvent(soundName, gameObject);
    }
}
