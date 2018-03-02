using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int collisions = -1;
    public float rotation = 0;
    public Rigidbody2D projectileRB;
    public GameObject frog;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collisions++;
            Debug.Log(collision.ToString());
            if (collisions > 0)
            {
                //doesnt work yet
                //Debug.Log("Destroy frog");
                // Destroy(frog);
            }
        }
    }
}