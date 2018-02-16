using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharPanel : MonoBehaviour {
    public Sprite Portrait;

    public Image[] players;

    public Image SelCoin;


    public CharSelectController charSelectController;

    public string CharName;
    public int Index;



    private bool selected = false;
    private bool[] playersOn = { false, false, false, false };
    private int selPlayer = -1;


    private void Start()
    {
        for (int i = 0; i < players.Length; i++) players[i].sprite = charSelectController.GetPlayerCoin(i + 1);
        ClearActivePlayers();
        SelCoin.gameObject.SetActive(false);
    }

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
        playersOn[selPlayer - 1] = false;
        SetCurrActivePlayers();
        SelCoin.gameObject.SetActive(true);
        SetSelBy(playerNum);
    }
    public void DeSelect(bool disconnected)
    {
        if (!disconnected) playersOn[selPlayer - 1] = true;
        selected = false;
        selPlayer = -1;
        SetCurrActivePlayers();
        SelCoin.gameObject.SetActive(false);
    }

    public void AddOn(int playerNum)
    {
        playersOn[playerNum - 1] = true;
        SetCurrActivePlayers();
    }
    public void RemoveOn(int playerNum)
    {
        playersOn[playerNum - 1] = false;
        SetCurrActivePlayers();
    }

    private void SetSelBy(int playerNum)
    {
        SelCoin.sprite = charSelectController.GetPlayerCoin(playerNum);
    }


    private void ClearActivePlayers()
    {
        foreach (Image i in players) i.gameObject.SetActive(false);
    }

    private void SetCurrActivePlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.SetActive(playersOn[i]);
        }
    }
}
