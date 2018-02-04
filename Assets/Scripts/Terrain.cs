using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour {

    public Transform t;
    public float tRotation;
    public float currentRotation;

  
    
	// Use this for initialization
	void Start () {
        tRotation = t.rotation.z;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)))
        {
            if (this.tag == "Grass" && collision.gameObject.GetComponent<Player>().GetIsFlying() == false)
            {
                collision.gameObject.GetComponent<Player>().terrainSpeed = 0.5f;
            }
            else if (this.tag == "Oil" && collision.gameObject.GetComponent<Player>().GetIsFlying() == false)
            {
                collision.gameObject.GetComponent<Player>().terrainTurning = 0.25f;
            }
            else if (this.tag == "Boost") 
            {
                currentRotation = collision.gameObject.GetComponent<Player>().playerRB.rotation;
                while (currentRotation < -360)
                    currentRotation += 360;
                if (currentRotation >= 360)
                    currentRotation = currentRotation % 360;

                tRotation = t.rotation.eulerAngles.z;


               /* while (tRotation < -360)
                    tRotation += 360;
                if (tRotation >= 360)
                    tRotation = tRotation % 360;*/
                Debug.Log(currentRotation);
                Debug.Log(tRotation);

                //collision.gameObject.GetComponent<Player>().maxSpeed = newMaxSpeed;
                if ((currentRotation >= tRotation - 90 && currentRotation <= tRotation + 90) ||
                    (currentRotation >= tRotation - 450 && currentRotation <= tRotation - 270))
                {
                    collision.gameObject.GetComponent<Player>().state = Player.STATES.BOOST;
                    collision.gameObject.GetComponent<Player>().SetBoost(Player.BOOSTS.BOOST_PAD, 1);
                }
                else
                    collision.gameObject.GetComponent<Player>().state = Player.STATES.BOOST_B;

               // collision.gameObject.GetComponent<Player>().terrainSpeed = 50f;
               //collision.gameObject.GetComponent<Player>().playerRB.rotation = 
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

            }
        }
        
    }
}
