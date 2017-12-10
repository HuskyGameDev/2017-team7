using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEscQuit : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Barnout.Quit();
        }
    }
}

