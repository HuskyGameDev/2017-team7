using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharPanel : MonoBehaviour {
    public Sprite Portrait;

    public string CharName;
    public int Index;
    private bool selected = false;
    private bool[] playersOn = { false, false, false, false };
    private int selPlayer = -1;



   
    public bool IsSelected()
    {
        return selected;
    }
    public int SelectedBy()
    {
        return selPlayer;
    }
    public void Select(int playerNum)
    {
        selected = true;
        selPlayer = playerNum;
    }
    public void DeSelect()
    {
        selected = false;
        selPlayer = -1;
    }

    public void AddOn(int playerNum)
    {
        playersOn[playerNum] = true;
    }
    public void RemoveOn(int playerNum)
    {
        playersOn[playerNum] = false;
    }

    public Image GetPortrait()
    {
        return null;
    }
}
