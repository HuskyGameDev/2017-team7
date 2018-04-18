using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour {
    protected void DoEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }
    private void FixedUpdate()
    {
        AkSoundEngine.RenderAudio();
    }
}
