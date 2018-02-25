using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int collisions = -1;
    public float rotation = 0;
    public Rigidbody2D projectileRB;

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
        collisions++;
        Debug.Log(collisions);
        if (collisions > 0)
        {
            //doesnt work yet
            Debug.Log("Destroy this");
            Destroy(this);
        }
    }
}