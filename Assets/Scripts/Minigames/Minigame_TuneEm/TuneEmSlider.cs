﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEmSlider : MonoBehaviour {
    public TuneEm minigame;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(0, minigame.GetLinePos() * 5);
	}
}
