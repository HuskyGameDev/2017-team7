using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinigamePlayer : MonoBehaviour {

    protected float score = 0;
    public int playerNum;
    protected bool isActive;
    protected bool initialized = false;
    protected Controller controller;

    public virtual void PlayerInit()
    {
        initialized = true;
        controller = Inputs.GetController(playerNum);
        Init();
    }

    protected abstract void Init();

    public virtual int CompareScore(MinigamePlayer p)
    {
        return isActive? score.CompareTo(p.score) : -1;
    }

    public float GetScore()
    {
        return score;
    }

    protected abstract void Tick();

	// Use this for initialization
	void Awake () {
        isActive = PlayerData.instance.playerChars[playerNum - 1] >= 0;
        controller = Inputs.GetController(playerNum);
	}

    private void FixedUpdate()
    {
        if (isActive && initialized) Tick();
    }

    public bool IsActive() { return isActive; }

}
