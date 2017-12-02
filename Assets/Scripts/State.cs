using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour {

    public Text stateText;
    public Player thisPlayer;

    // Use this for initialization
    void Start()
    {
        stateText.text = "State: ###";
    }

    // Update is called once per frame
    void Update()
    {
        stateText.text = "State: " + thisPlayer.state;
    }
}

