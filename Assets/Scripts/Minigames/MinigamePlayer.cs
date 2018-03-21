using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePlayer : MonoBehaviour {

    protected float score = 0;
    public int playerNum;

    public virtual int CompareScore(MinigamePlayer p)
    {
        return score.CompareTo(p.score);
    }

    public float getScore()
    {
        return score;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
