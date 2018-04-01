using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EELtrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision) {
<<<<<<< HEAD
		if (collision.GetType().Equals(typeof(CapsuleCollider2D))) {
            if (collision.gameObject.GetComponent<Player>().state != Player.STATES.FLYING)
            {
                collision.gameObject.GetComponent<Player>().StartIncap(2);
            }
		}	
=======
		if (collision.tag == "PlayerWallCollider") {
            collision.gameObject.GetComponentInParent<Player>().StartIncap(2);
		}

		if (collision.GetType().Equals(typeof(CircleCollider2D))) {
			//Todo: make the players bounce off of each other if they both have the eel powerup activated.
		}		
>>>>>>> 0cc2186ea9c092c02d9200ba8081544b63406c15
	}

	//Also need to add a method that does stuff (if anything) on the collider exit.
}
