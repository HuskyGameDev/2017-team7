using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinigamePlayer : MonoBehaviour {

    protected float score = 0;
    public int playerNum;
    protected bool isActive;
    protected bool initialized = false;
    protected Controller controller;
    protected bool finished;

    public void PlayerInit()
    {
        initialized = true;
        controller = Inputs.GetController(playerNum);
        Init();
    }

    protected abstract void Init();

    public virtual int CompareScore(MinigamePlayer p)
    {
        if (isActive)
        {
            return p.IsActive() ? -score.CompareTo(p.score) : -1;
        }
        return 1;
    }

    public float GetScore()
    {
        return score;
    }

    protected abstract void Tick();

	// Use this for initialization
	void Awake () {
        isActive = PlayerData.instance.barnoutPlayers[playerNum - 1].IsActive();
        controller = Inputs.GetController(playerNum);
	}

    private void FixedUpdate()
    {
        if (isActive && initialized && controller != null && !finished) Tick();
    }

    public bool IsActive() { return isActive; }

    public void Finish() {
        finished = true;
        if (isActive) OnGameDone();
    }

    protected abstract void OnGameDone();

}
