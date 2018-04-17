using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneController : MonoBehaviour {

    public EndScenePlayer[] players;

    private bool done;

    public Sprite[] winnerSprites;
    public Sprite[] loserCoins;
    public Sprite[] playerCoins;

	// Use this for initialization
	void Start ()
    {
        done = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!done)
        {
            done = true;
            foreach (EndScenePlayer p in players)
            {
                if (p.GetState() == EndScenePlayerState.SELECTING)
                {
                    done = false;
                    break;
                }
            }


            if (done)
            {
                bool rematch = true;
                foreach (EndScenePlayer p in players)
                {
                    if (p.GetState() != EndScenePlayerState.REMATCH)
                    {
                        rematch = false;
                        break;
                    }
                }
                if (rematch)
                {
                    StartCoroutine(BackToMainMenu());
                }
                else
                {
                    StartCoroutine(StartRematch());
                }
            }
        }
        
    }

    IEnumerator BackToMainMenu()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        Barnout.ChangeScene("MainMenu");
    }

    IEnumerator StartRematch()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        List<BarnoutPlayer> barnoutPlayers = new List<BarnoutPlayer>();
        foreach(BarnoutPlayer p in PlayerData.instance.barnoutPlayers)
        {
            barnoutPlayers.Add(new BarnoutPlayer(p.IsActive(), p.GetCharacter(), p.GetPlayerNum()));
        }
        PlayerData.Instantiate(barnoutPlayers.ToArray());
        Barnout.ChangeScene("MinigameStart");
        //TODO do rematch code
    }



    public Sprite GetWinnerSprite(int charNum)
    {
        return winnerSprites[charNum];
    }

    public Sprite GetPlayerCoin(int playerNum)
    {
        return playerCoins[playerNum - 1];
    }

    public Sprite GetLoserCoin(int charNum)
    {
        return loserCoins[charNum];
    }

}
