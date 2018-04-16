using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCoins : MonoBehaviour {
    public TransitionCoinStanding[] standings;
    public Sprite[] charImages;
    public Sprite[] playerCoinImages;

    public void Init()
    {
        foreach(MinigameData.Standing s in MinigameData.standings)
        {
            SetPlace(s);
        }
    }


    private void SetPlace(MinigameData.Standing standing)
    {
        int playerIndex = standing.playerNumber - 1;
        int standingIndex = standing.standing - 1;
        TransitionCoinStanding tcStanding = standings[standing.standing - 1]; //-1 for 0 index
        BarnoutPlayer currPlayer = PlayerData.instance.barnoutPlayers[playerIndex];
        if (currPlayer.IsActive())
        {
            tcStanding.SetCoins(playerCoinImages[playerIndex], charImages[currPlayer.GetCharacter()]);
        }
        else
        {
            tcStanding.gameObject.SetActive(false);
        }
    }



}
