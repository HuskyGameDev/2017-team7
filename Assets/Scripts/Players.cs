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

    private void Start()
    {
        playerCount = PlayerData.playerChars.Count(x => x >=0);
        int index = 0;
        Debug.Log(playerCount);
        for (int i = 0; i < PlayerData.playerChars.Length;  i++)
        {
            int playerChar = PlayerData.playerChars[i];
            if (playerChar >= 0)
            {
                Player p = players[i];
                p.animator.runtimeAnimatorController = playerControllers[playerChar];
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

        PlayerData.players = players;
    }
}
