using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour {

    public Transform t;
    public float tRotation;
    public float currentRotation;
    public Player owner;
    int collisions = 0;

  
    
	// Use this for initialization
	void Start () {
        tRotation = t.rotation.z;
        StartCoroutine(clearOwner(2));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerWallCollider")
        {
<<<<<<< HEAD
            if (collision.gameObject.GetComponent<Player>().state != Player.STATES.FLYING)
            {
                if (this.tag == "Grass")
                {
                    collision.gameObject.GetComponent<Player>().terrainSpeed = 0.5f;
                }
                else if (this.tag == "Oil")
                {
                    if (collision.gameObject.GetComponent<Player>().state != Player.STATES.MOVE_B &&
                        collision.gameObject.GetComponent<Player>().state != Player.STATES.ACCEL &&
                        collision.gameObject.GetComponent<Player>() != owner)
                    {
                        collisions++;
                        collision.gameObject.GetComponent<Player>().setDriftDir(collision.gameObject.GetComponent<Player>().playerRB.rotation);
                        collision.gameObject.GetComponent<Player>().state = Player.STATES.OILED;
                        if (collisions >= 3)
                            Destroy(gameObject);
                    }
                }
                else if (this.tag == "Boost")
                {
                    currentRotation = collision.gameObject.GetComponent<Player>().playerRB.rotation;
                    while (currentRotation < -360)
                        currentRotation += 360;
                    if (currentRotation >= 360)
                        currentRotation = currentRotation % 360;
=======
            if (this.tag == "Grass")
            {
                collision.gameObject.GetComponentInParent<Player>().terrainSpeed = 0.5f;
            }
            else if (this.tag == "Oil")
            {
                if (collision.gameObject.GetComponentInParent<Player>().state != Player.STATES.MOVE_B &&
                    collision.gameObject.GetComponentInParent<Player>().state != Player.STATES.ACCEL && 
                    collision.gameObject.GetComponentInParent<Player>() != owner)
                {
                    collisions++;
                    collision.gameObject.GetComponentInParent<Player>().setDriftDir(collision.gameObject.GetComponentInParent<Player>().playerRB.rotation);
                    collision.gameObject.GetComponentInParent<Player>().state = Player.STATES.OILED;
                    if (collisions >= 3)
                        Destroy(gameObject);
                }
            }
            else if (this.tag == "Boost") 
            {
                currentRotation = collision.gameObject.GetComponentInParent<Player>().playerRB.rotation;
                while (currentRotation < -360)
                    currentRotation += 360;
                if (currentRotation >= 360)
                    currentRotation = currentRotation % 360;
>>>>>>> 0cc2186ea9c092c02d9200ba8081544b63406c15

                    tRotation = t.rotation.eulerAngles.z;

<<<<<<< HEAD
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
=======
                //if facing forward
                if ((currentRotation >= tRotation - 90 && currentRotation <= tRotation + 90) ||
                    (currentRotation >= tRotation - 450 && currentRotation <= tRotation - 270))
                {
                    collision.gameObject.GetComponentInParent<Player>().StartBoost(Player.BOOSTS.PAD, 1);
                }
                //if facing backward
                else
                    collision.gameObject.GetComponentInParent<Player>().StartBoostB(Player.BOOSTS.PAD_BACK, 1);
>>>>>>> 0cc2186ea9c092c02d9200ba8081544b63406c15
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerWallCollider")
        {
            if (this.tag == "Grass")
            {
                collision.gameObject.GetComponentInParent<Player>().terrainSpeed = 1;
            }
            else if (this.tag == "Oil")
            {
                
            }
            else if (this.tag == "Boost") 
            {

            }
        }      
    }

    IEnumerator clearOwner(float time)
    {
        yield return new WaitForSeconds(time);
        owner = null;
    }
}
