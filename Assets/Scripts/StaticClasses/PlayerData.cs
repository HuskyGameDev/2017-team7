using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData
{

    public static PlayerData instance = new PlayerData();
    public static void Instantiate(BarnoutPlayer[] newBarnoutPlayers)
    {
        instance = new PlayerData(newBarnoutPlayers);
    }

    PlayerData(BarnoutPlayer[] newBarnoutPlayers)
    {
        barnoutPlayers = newBarnoutPlayers;
        numPlayers = barnoutPlayers.Count(x => x.IsActive());
    }
    PlayerData() 
        : this( new BarnoutPlayer[] { 
            new BarnoutPlayer(true, 0, 1),
            new BarnoutPlayer(true, 1, 2),
            new BarnoutPlayer(false, 0, 3),
            new BarnoutPlayer(false, 0, 4)
        }) {}

    public BarnoutPlayer GetFirstActivePlayer()
    {
        foreach(BarnoutPlayer p in barnoutPlayers)
        {
            if (p.IsActive()) return p;
        }
        return null;
    }

    public int numPlayers;
    //public int[] playerChars;
    public BarnoutPlayer[] barnoutPlayers;
    //public static Sprite[] charIcons;
    
    public Player[] players;
}
