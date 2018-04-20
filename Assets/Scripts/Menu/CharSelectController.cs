using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectController : MonoBehaviour {

    public PlayerPanel[] players;
    public CharPanel[] chars;

    public Sprite[] PlayerCoins;
     
    //private string[] CharNames = { "Beefcake", "Fat Stacks", "Sheepish", "Vainglory" };

    private string SuperSecretMinigameOMGSceneName = "JoshDev2";

    private bool CanSelect(int charNum)
    {
        return !chars[charNum].IsSelected();
    }
    
    public bool SelectChar(int charNum, int playerNum)
    {
        if (CanSelect(charNum))
        {
            chars[charNum].Select(playerNum);
            return true;
        }
        return false;
    }

    public void DeSelectChar(int charNum, bool disconnected = false)
    {
        chars[charNum].DeSelect(disconnected);
    }

    private bool CanStart()
    {
        int count = 0;
        foreach (PlayerPanel p in players)
        {
            if (p.IsSelecting())
            {
                return false;
            }
            else if (p.IsReady())
            {
                count++;
            }
        }
        return count >= 2;
    }


    public CharPanel NextChar(int index)
    {
        int nextIndex = (index + 1) % chars.Length;
        if (chars[nextIndex].IsSelected()) return NextChar(nextIndex);
        return chars[nextIndex];
    }

    public CharPanel PrevChar(int index)
    {
        int prevIndex = (index - 1 < 0) ? chars.Length - 1 : index - 1;
        if (chars[prevIndex].IsSelected()) return PrevChar(prevIndex);
        return chars[prevIndex];
    }

    public void AddOn(int charNum, int playerNum)
    {
        chars[charNum].AddOn(playerNum);
    }
    public void RemoveOn(int charNum, int playerNum)
    {
        chars[charNum].RemoveOn(playerNum);
    }

    public void StartGame()
    {
        if (CanStart())
        {
            BarnoutPlayer[] barnoutPlayers = new BarnoutPlayer[players.Length];
            
            for(int i = 0; i < barnoutPlayers.Length; i++)
            {
                if (players[i].IsReady())
                {
                    barnoutPlayers[i] = new BarnoutPlayer(true, players[i].SelectedChar(), i + 1);
                }
                else
                {
                    barnoutPlayers[i] = new BarnoutPlayer(false, -1, i + 1);
                }
            }

            PlayerData.Instantiate((BarnoutPlayer[])barnoutPlayers.Clone());
            //PlayerData.charIcons = (Sprite[])images.Clone();
            //PlayerData.charTopDowns = (Sprite[])playerTopDowns.Clone();
            string[] maps = { "MainScene", "MainScene2" };
            if (MenuAudio.Instance) MenuAudio.Instance.StopMusic();
            EndData.Instantiate();
            //Barnout.ChangeScene(maps[Random.Range(0, maps.Length)]);
            if(ShouldActivateSuperSecretMode(barnoutPlayers)){
                Barnout.ChangeScene(SuperSecretMinigameOMGSceneName);
            }else{
                Barnout.ChangeScene("MinigameStart");
            }
        }
    }

    public Sprite GetPlayerCoin(int playerNum)
    {
        return PlayerCoins[playerNum - 1];
    }

    private bool ShouldActivateSuperSecretMode(BarnoutPlayer[] barnoutPlayers){
        BarnoutPlayer firstPlayer = barnoutPlayers.First( x => x.IsActive());
        return  Barnout.ButtonHeld(players[firstPlayer.GetPlayerNum() - 1].gp_state.Buttons.RightShoulder) &&
                Barnout.ButtonHeld(players[firstPlayer.GetPlayerNum() - 1].gp_state.Buttons.LeftShoulder);
    }
}
