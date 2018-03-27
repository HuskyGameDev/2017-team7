using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEmBG : MonoBehaviour {

    public Animator[] panels;

	// Use this for initialization
	public void Init () {
        int startVal = 3;
        foreach (Animator panel in panels)
        {
            panel.SetInteger("BeginNum", startVal);
            startVal--;
        }
	}
	
    public void Increment()
    {
        foreach(Animator panel in panels)
        {
            int beginNum = panel.GetInteger("BeginNum");
            if (beginNum >= 1)
            {
                panel.SetTrigger("ToNext");
            }
            else
            {
                panel.SetInteger("BeginNum", beginNum + 1);
            }
        }
    }
}
