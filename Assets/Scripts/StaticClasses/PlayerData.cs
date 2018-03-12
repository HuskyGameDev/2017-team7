using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData
{

    public static PlayerData instance = new PlayerData();
    public void Instantiate()
    {
        instance = new PlayerData();
    }

    PlayerData()
    {
        numPlayers = playerChars.Count(x => x >= 0);
    }

    public int numPlayers;
    public int[] playerChars = { 0, -1, 2, 3 };
    //public static Sprite[] charIcons;
    
    public Player[] players;
}
