using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData
{

    public static PlayerData instance = new PlayerData();
    public static void Instantiate(int[] newPlayerChars)
    {
        instance = new PlayerData(newPlayerChars);
    }

    PlayerData(int[] newPlayerChars)
    {
        playerChars = newPlayerChars;
        numPlayers = playerChars.Count(x => x >= 0);
    }
    PlayerData() 
        : this( new int[] { 0,3, -1, -1 }) {

    }

    public int numPlayers;
    public int[] playerChars;
    //public static Sprite[] charIcons;
    
    public Player[] players;
}
