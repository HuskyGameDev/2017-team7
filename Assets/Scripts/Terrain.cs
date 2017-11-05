using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour {


    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)))
        {
            if (this.tag == "Grass")
            {
                collision.gameObject.GetComponent<Player>().terrainSpeed = 0.5f;
            }
            else if (this.tag == "Oil")
            {

                collision.gameObject.GetComponent<Player>().terrainTurning = 0.25f;
            }
            else if (this.tag == "Boost") { }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)))
        {
            if (this.tag == "Grass")
            {
                collision.gameObject.GetComponent<Player>().terrainSpeed = 1;
            }
            else if (this.tag == "Oil")
            {
                collision.gameObject.GetComponent<Player>().terrainTurning = 1;
            }
        }
    }
}
