using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameIntro : MonoBehaviour {

    Minigame minigame;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    bool AnimReady = false;
    bool AnimDone = false;
	public void FinishAnim()
    {
        AnimDone = true;
    }
    public void StartReady()
    {
        AnimReady = true;
    }

    public void SkipIntro()
    {
        Debug.Log("Intro skipped");
        if (animator == null) animator = GetComponent<Animator>();
        animator.SetBool("SkipIntro", true);
    }

    public bool IsReady()
    {
        return AnimReady;
    }

    public bool IsDone()
    {
        return AnimDone;
    }

    public void StartOutro()
    {
        animator.SetTrigger("ToOutro");
    }

    public void GoToNextScene()
    {
        Barnout.ChangeScene("MinigameTransition");
    }

}
