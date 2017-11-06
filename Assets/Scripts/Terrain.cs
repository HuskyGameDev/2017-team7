using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour {

    private float oldMaxSpeed = 125;
    private float newMaxSpeed = 175;
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator waitOneSecond(Collider2D collision)
    {
        yield return new WaitForSeconds(1);
        collision.gameObject.GetComponent<Player>().maxSpeed = oldMaxSpeed;
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
            else if (this.tag == "Boost") 
            {
                //oldMaxSpeed = collision.gameObject.GetComponent<Player>().maxSpeed;
                collision.gameObject.GetComponent<Player>().maxSpeed = newMaxSpeed;
                collision.gameObject.GetComponent<Player>().terrainSpeed = 50f;

            }
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
            else if (this.tag == "Boost") 
            {
                //float diff = newMaxSpeed - oldMaxSpeed;
                //float ratio = diff / oldMaxSpeed;
                StartCoroutine(waitOneSecond(collision));
                collision.gameObject.GetComponent<Player>().terrainSpeed = 1;


               // while (collision.gameObject.GetComponent<Player>().maxSpeed > oldMaxSpeed) {
                   // collision.gameObject.GetComponent<Player>().maxSpeed -= ratio;
               // }
            }
        }
        
    }
}
