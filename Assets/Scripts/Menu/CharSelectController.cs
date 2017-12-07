using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectController : MonoBehaviour {

    public Sprite[] images;
    public Sprite[] playerTopDowns;

    private int[] SelectedChars = { -1, -1, -1, -1 };
    private string[] CharNames = { "Beefcake", "Fat Stacks", "Sheepish", "Vainglory" };

    public void selectCharacter(int playernum, int index) {
        SelectedChars[playernum] = index;
    }
    public void deselectCharacter(int playernum)
    {
        SelectedChars[playernum] = -1;
    }
    
    public Sprite getCharImage(int index)
    {
        return images[index];
    }

    public string getCharName(int index)
    {
        return CharNames[index];
    }

    public bool charSelected(int index)
    {
        for (int i = 0; i < SelectedChars.Length; i++)
        {
            if (SelectedChars[i] == index) return true;
        }
        return false;
    }

    public int nextChar(int index)
    {
        int next = index + 1;
        if (next >= SelectedChars.Length) next = 0;
        return next;
    }
    public int prevChar(int index)
    {
        int next = index - 1;
        if (next < 0) next = SelectedChars.Length - 1;
        return next;
    }

    public bool canStart()
    {
        int readyPlayers = 0;
        for (int i = 0; i < SelectedChars.Length; i++)
        {
            if (SelectedChars[i] >= 0)
            {
                readyPlayers++;
                if (readyPlayers > 1) return true;
            }
        }
        return false;
    }

    public void startGame()
    {
        if (canStart())
        {
            Debug.Log("Start");
            PlayerData.playerChars = (int []) SelectedChars.Clone();
            PlayerData.charIcons = (Sprite[])images.Clone();
            PlayerData.charTopDowns = (Sprite[])playerTopDowns.Clone();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
    }
}
