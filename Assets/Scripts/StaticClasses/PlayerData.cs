using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int[] playerChars = { 0, 1, -1, -1 };
    public static Sprite[] charTopDowns;
    public static Sprite[] charIcons;
    
    public static Player[] players;

    public static Player[] GetActivePlayers(){
        int count = 0;
        Player[] active;
        foreach(int i in playerChars){
            if(i < 0){
                count++;
            }
        }

        active = new Player[count];

        for(int i = 0; i < playerChars.Length;i++){
            if(playerChars[i] >= 0){
                foreach(Player p in players){
                    if(p.playerNumber == i+1){
                        active[i] = p; 
                        break;
                    }
                }
            }
        }

        return active;
    }
}
