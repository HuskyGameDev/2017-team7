using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneEmPlayer : MonoBehaviour {

    public int playerNum;
    public TuneEm minigame;

    bool failed = false;
    bool pressed;
    float numPressed = 0;

    Controller controller;
    bool aBuffer = true;
	// Use this for initialization
	void Start () {
        controller = Inputs.GetController(playerNum);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!pressed)
        {
            if (controller.GetA() && aBuffer)
            {
                pressed = true;
                if (minigame.InBounds())
                {
                    Success();
                }
                else
                {
                    Fail();
                }
            }
        }
        
	}

    void Success()
    {
        //animator.SetTrigger("Crank");
        numPressed += 1;
    }
    public void Fail()
    {
        failed = true;
        if (pressed) numPressed += minigame.GetCloseScore();
        //animator.SetTrigger("Fail");
    }

    public bool HasPressed()
    {
        return pressed;
    }
    public float NumPressed()
    {
        return numPressed;
    }
    public int ComparePressed(TuneEmPlayer p)
    {
        return numPressed.CompareTo(p.NumPressed());
    }
    public void Reset()
    {
        if (!HasFailed())
        {
            pressed = false;
        }
        

    }
    public bool HasFailed()
    {
        return failed;
    }
}
