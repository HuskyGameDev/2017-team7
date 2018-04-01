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

		if (collision.tag == "PlayerWallCollider") {
            if (collision.gameObject.GetComponentInParent<Player>().state != Player.STATES.FLYING)
            {
                collision.gameObject.GetComponentInParent<Player>().StartIncap(2);
            }
		}	

	}

	//Also need to add a method that does stuff (if anything) on the collider exit.
}
