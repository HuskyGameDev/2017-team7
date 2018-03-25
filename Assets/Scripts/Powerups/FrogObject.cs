using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogObject : Projectile {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)) && collision.gameObject.tag == "Player")
        {
            collisions++;
            Debug.Log("Collisions: " + collision.ToString());
            if (collisions > 0)
            {
                collision.gameObject.GetComponent<Player>().StartIncap(2);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.tag == "wall")
        {
            Debug.Log(this.ToString() + " Hit wall");
            Destroy(gameObject);
        }
    }
}
