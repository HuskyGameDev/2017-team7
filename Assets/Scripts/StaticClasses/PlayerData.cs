using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int numPlayers = 4;
    public static int[] playerChars = { 0, 1, 2, 3 };

    public static Sprite[] charIcons;
    
    public static Player[] players;

    private static Player[] activePlayers = null;

    public static Player[] GetActivePlayers(){
        if(activePlayers != null){
            return activePlayers;
        }

        int count = 0;
        Player[] active;
        foreach(int i in playerChars){
            if(i >= 0){
                count++;
            }
        }

        active = new Player[count];
        count = 0;

        for(int i = 0; i < playerChars.Length;i++){
            if(playerChars[i] >= 0){
                foreach(Player p in players){
                    if(p.playerNumber == i+1){
                        active[count++] = p; 
                        break;
                    }
                }
            }
        }
        
        activePlayers = active;
        return active;
    }

    public static Player GetPlayerByNumber(int i)
    {
        Player[] temp = GetActivePlayers();

        foreach (Player p in temp)
        {
            if (p.playerNumber == i)
            {
                return p;
            }
        }

        return null;
    }
}
