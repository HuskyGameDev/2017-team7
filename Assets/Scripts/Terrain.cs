using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour {

    public Transform t;
    public float tRotation;
    public float currentRotation;

    public int collisions = -1;

  
    
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
            collisions++;
            if (this.tag == "Grass")
            {
                collision.gameObject.GetComponent<Player>().terrainSpeed = 0.5f;
            }
            else if (this.tag == "Oil")
            {
                if (collision.gameObject.GetComponent<Player>().state != Player.STATES.MOVE_B &&
                    collision.gameObject.GetComponent<Player>().state != Player.STATES.ACCEL && collisions > 0)
                {
                    collision.gameObject.GetComponent<Player>().setDriftDir(collision.gameObject.GetComponent<Player>().playerRB.rotation);
                    collision.gameObject.GetComponent<Player>().state = Player.STATES.OILED;
                }
            }
            else if (this.tag == "Boost") 
            {
                currentRotation = collision.gameObject.GetComponent<Player>().playerRB.rotation;
                while (currentRotation < -360)
                    currentRotation += 360;
                if (currentRotation >= 360)
                    currentRotation = currentRotation % 360;

                tRotation = t.rotation.eulerAngles.z;

                //if facing forward
                if ((currentRotation >= tRotation - 90 && currentRotation <= tRotation + 90) ||
                    (currentRotation >= tRotation - 450 && currentRotation <= tRotation - 270))
                {
                    collision.gameObject.GetComponent<Player>().StartBoost(Player.BOOSTS.PAD, 1);
                }
                //if facing backward
                else
                    collision.gameObject.GetComponent<Player>().StartBoostB(Player.BOOSTS.PAD_BACK, 1);
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
                
            }
            else if (this.tag == "Boost") 
            {

            }
        }
        
    }
}
