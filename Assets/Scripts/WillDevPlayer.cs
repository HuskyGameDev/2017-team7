using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D playerRB;

    private Controller ctrls;

    public float turnIncr;
    public float turningSpeed;
    private float maxTS;
    private float turnSp = 0;

    public float acceleration;
	public float maxSpeed;
    Vector2 prevVel;

    private CapsuleCollider2D collider;
    private BoxCollider2D draftingHitbox;
    private bool drafting = false;
    private int draftTime = 0;
    private float draftBoost = 1;

	public string playerNumber;

    public PhysicsMaterial2D wallMaterial, playerMaterial;

    // Use this for initialization
    void Start()
    { 
        playerRB = GetComponent<Rigidbody2D>();
        ctrls = Inputs.GetController(Convert.ToInt32(playerNumber));
        playerRB.freezeRotation = true;
        collider = GetComponent<CapsuleCollider2D>();
        draftingHitbox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Add rotation
        turnSp += ctrls.GetTurn() * turnIncr;
        maxTS = Math.Abs(ctrls.GetTurn() * turningSpeed);
        turnSp = Math.Min(Math.Max(-maxTS, turnSp), maxTS);
        playerRB.rotation += turnSp;

       // if(ctrls.GetTurn() != 0 && turnSp != maxTS) Debug.Log("Turn: " + ctrls.GetTurn() + " turnSp: " + turnSp + " maxTS: " + maxTS );

        Vector2 newVel = new Vector2();
		Vector2 accel = new Vector2 ();
		// We get the rotation, convert to radians, and also add 90 degrees (PI/2 radians) to get our direction angle.
		float rotation = (float)(Math.PI/180.0) * playerRB.rotation + (float)(Math.PI/2.0);
		//new vel should be in the direction of rotation
		newVel.Set ((float)Math.Cos (rotation), (float)Math.Sin (rotation));
        
		accel = newVel * acceleration * draftBoost * ctrls.GetSpeed();


		//Clamp to max speed
		newVel = Vector2.ClampMagnitude((newVel * playerRB.velocity.magnitude) + accel, maxSpeed);

        prevVel = newVel;
		playerRB.velocity = newVel;
    }

    void FixedUpdate()
    {
        if (drafting && playerRB.velocity.magnitude > (maxSpeed / 2))
        {
            draftTime++;
            if (draftTime > 125) drafting = false;
        }
        else if (!drafting && draftTime > 0)
        {
            draftBoost = 3;
            draftTime--;
        }
        else
        {
            draftBoost = 1;
        }
        Debug.Log("Player: " + playerNumber + "DraftTime: " + draftTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)))
        //if (collision.gameObject.tag == "Player")
        {
            drafting = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    { 
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)))
           // if (collision.gameObject.tag == "Player")
        {
            drafting = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            //collider.sharedMaterial.bounciness = 10;
           // Debug.Log("Hit player");
        }
        else if (collision.gameObject.tag == "wall")
        {
            
           // Debug.Log("Hit wall");     
        }
    }
}
