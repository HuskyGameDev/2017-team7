using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogObject : Projectile {

    public bool toDestroy = false;

	// Use this for initialization
	void Start () {
		if (toDestroy)
        {
            Destroy(gameObject, 10);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerWallCollider")
        {
            if (collision.gameObject.GetComponentInParent<Player>().state != Player.STATES.FLYING)
            {

                collisions++;
                Debug.Log("Collisions: " + collision.ToString());
                if (collisions > 0)
                {
                    collision.gameObject.GetComponentInParent<Player>().StartIncap(2);
                    Destroy(gameObject);
                }

            }
        }
        else if (collision.gameObject.tag == "wall")
        {
            Debug.Log(this.ToString() + " Hit wall");
            Destroy(gameObject);
        }
    }
}
