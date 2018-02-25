using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public RuntimeAnimatorController[] animatorControllers;


    private void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            int playerChar = PlayerData.playerChars[i];
            if (playerChar >= 0)
            {
                players[i].animator.runtimeAnimatorController = animatorControllers[playerChar];
            }
            else
            {
                players[i].gameObject.SetActive(false);
            }
        }
    }
}
