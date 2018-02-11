using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MinigameController : MonoBehaviour {
    
    private Minigame currentMinigame;

    // Use this for initialization
	public void SetMinigame(Minigame m){
        currentMinigame = m;
    }

    void Awake() {
        
    }

}