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
        if (collision.tag == "PlayerWallCollider")
        {
            if (collision.gameObject.GetComponent<Player>().state != Player.STATES.FLYING)
            {
<<<<<<< HEAD
                collisions++;
                Debug.Log("Collisions: " + collision.ToString());
                if (collisions > 0)
                {
                    collision.gameObject.GetComponent<Player>().StartIncap(2);
                    Destroy(gameObject);
                }
=======
                collision.GetComponentInParent<Player>().StartIncap(2);
                Destroy(gameObject);
>>>>>>> 0cc2186ea9c092c02d9200ba8081544b63406c15
            }
        }
        else if (collision.gameObject.tag == "wall")
        {
            Debug.Log(this.ToString() + " Hit wall");
            Destroy(gameObject);
        }
    }
}
