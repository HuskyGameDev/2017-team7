using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuButton : MonoBehaviour {

    Animator animator;

	void Awake () {
        animator = GetComponent<Animator>();
	}

    public abstract void ActivateButton();

    public void ToHover()
    {
        animator.SetTrigger("ToHover");
    }
    public void ToUnselected()
    {
        animator.SetTrigger("ToUnselected");
    }
}
