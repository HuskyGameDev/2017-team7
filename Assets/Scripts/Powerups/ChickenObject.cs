﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenObject : MonoBehaviour
{
    public float rotation = 0;
    public Player owner;
    int collisions = 0;

    public EggSplat[] eggSplats;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(clearOwner(2));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)) && collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Player>() != owner)
        {
            collisions++;
            eggSplats[collision.gameObject.GetComponent<Player>().playerNumber - 1].enableSplat(2f);
            if (collisions >= 1)
                Destroy(gameObject);
        }
    }

    IEnumerator clearOwner(float time)
    {
        yield return new WaitForSeconds(time);
        owner = null;
    }
}
