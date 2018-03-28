using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameIntro : MonoBehaviour {

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    bool AnimDone = false;
	public void FinishAnim()
    {
        AnimDone = true;
    }

    public bool IsDone()
    {
        return AnimDone;
    }

    public void StartOutro()
    {
        animator.SetTrigger("ToOutro");
    }

}
