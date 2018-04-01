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
		if (collision.GetType().Equals(typeof(CapsuleCollider2D))) {
            if (collision.gameObject.GetComponent<Player>().state != Player.STATES.FLYING)
            {
                collision.gameObject.GetComponent<Player>().StartIncap(2);
            }
		}	
	}

	//Also need to add a method that does stuff (if anything) on the collider exit.
}
