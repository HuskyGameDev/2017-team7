using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePlayer : MonoBehaviour {

    protected float score = 0;
    public int playerNum;
    bool isActive;

    protected Controller controller;

    public virtual int CompareScore(MinigamePlayer p)
    {
        return isActive? score.CompareTo(p.score) : -1;
    }

    public float GetScore()
    {
        return score;
    }

	// Use this for initialization
	void Awake () {
        isActive = PlayerData.instance.playerChars[playerNum - 1] >= 0;
        controller = Inputs.GetController(playerNum);
	}
}
