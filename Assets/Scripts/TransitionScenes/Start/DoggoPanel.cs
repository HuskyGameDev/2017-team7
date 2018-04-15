using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoPanel {
    public enum EMOTION { HAPPY, NEUTRAL, ANNOYED, SLY, SAD, ECSTATIC, DETERMINED }
    private EMOTION _emotion;
    private string _text;
    private float _duration;

    public DoggoPanel(EMOTION emotion, string text, float duration)
    {
        _emotion = emotion;
        _text = text;
        _duration = duration;
    }

    public string GetText()
    {
        return _text;
    }

    public EMOTION GetEmotion()
    {
        return _emotion;
    }
    
    public float GetDuration()
    {
        return _duration;
    }
}
