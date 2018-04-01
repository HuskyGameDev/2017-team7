using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogObject : Projectile {

    public bool toDestroy = false;
    public Player owner;

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
        if (collision.tag == "PlayerWallCollider" && collision.gameObject.GetComponentInParent<Player>() != owner)
        {
            if (collision.gameObject.GetComponentInParent<Player>().state != Player.STATES.FLYING)
            {
                collision.gameObject.GetComponentInParent<Player>().StartIncap(2);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }
    }
}
