using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Players : MonoBehaviour {

    public float turnIncr;
    public float turningSpeed;
    public float acceleration;
    public float maxSpeed;
    public float maxReverse;

    public PhysicsMaterial2D wallMaterial, playerMaterial;

    public float terrainSpeed = 1;
    public float terrainTurning = 1;

    public Map mapEvents;
    public PowerupInstantiator powerupInstantiator;

    public Player[] players;
    public RuntimeAnimatorController[] playerControllers;
    public RuntimeAnimatorController[] cameraControllers2p;
    public RuntimeAnimatorController[] cameraControllers4p;

    public LapDisplayMaster lapDisplayMaster;

    private int playerCount;

    public int maxCamZoom2;
    public int minCamZoom2;
    public int maxCamZoom4;
    public int minCamZoom4;

    /* New physics stuff!*/
    

    private void Start()
    {
        playerCount = PlayerData.instance.numPlayers;
        int index = 0;
        for (int i = 0; i < PlayerData.instance.barnoutPlayers.Length;  i++)
        {
            BarnoutPlayer barnoutPlayer = PlayerData.instance.barnoutPlayers[i];         
            if (barnoutPlayer.IsActive())
            {
                Player p = players[i];
                p.animator.runtimeAnimatorController = playerControllers[barnoutPlayer.GetCharacter()];
                if (playerCount > 2)
                {
                    p.overheadCamera.SetAnimator(cameraControllers4p[i]);
                }
                else
                {
                    p.overheadCamera.SetAnimator(cameraControllers2p[index]);
                }
                index++;
            }
            else
            {
                
                players[i].gameObject.SetActive(false);
            }
        }

        PlayerData.instance.players = players;
    }
}
