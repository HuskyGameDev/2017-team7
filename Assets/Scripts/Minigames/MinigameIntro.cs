using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameIntro : MonoBehaviour {
    bool AnimDone = false;
	public void FinishAnim()
    {
        AnimDone = true;
    }

    public bool IsDone()
    {
        return AnimDone;
    }
}
