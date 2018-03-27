using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEmBG : MonoBehaviour {

    public Animator[] panels;
    public TuneEmPlayer player;
    public GameObject NotPlaying;
	// Use this for initialization
	public void Start () {
        int startVal = 3;
        foreach (Animator panel in panels)
        {
            if (true)
            {
                panel.SetInteger("BeginNum", startVal);
                startVal--;
            }
        }
        NotPlaying.SetActive(!player.IsActive());
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
