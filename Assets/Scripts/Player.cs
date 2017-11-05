using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D playerRB;

    private Controller ctrls;

    public float turnIncr;
    public float turningSpeed;
    private float maxTS;
    private float turnSp = 0;

    public float acceleration;
    public float maxSpeed;
    private float maxReverse = 15;

    private CapsuleCollider2D collider;
    private BoxCollider2D draftingHitbox;
    private bool drafting = false;
    private int draftTime = 0;
    private float draftBoost = 1;

    public string playerNumber;

    public PhysicsMaterial2D wallMaterial, playerMaterial;

    private int reverseWait = 10;
    private int wait;

    public float terrainSpeed = 1;
    public float terrainTurning = 1;

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

        // Pre check of direction to determine rotation direction
        Vector2 newVel = new Vector2();
        // We get the rotation, convert to radians, and also add 90 degrees (PI/2 radians) to get our direction angle.
        float rotation = (float)(Math.PI / 180.0) * playerRB.rotation + (float)(Math.PI / 2.0);
        //new vel should be in the direction of rotation
        newVel.Set((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        float turn;

        // Forward
        if (Vector2.Angle(playerRB.velocity, newVel) < 90)
        {
            turn = ctrls.GetTurn();
        }
        // Backward
        else
        {
            turn = -ctrls.GetTurn();
        }

        //Add rotation
        turnSp += turn * turnIncr;// * (playerRB.velocity.magnitude/(maxSpeed/2));
        maxTS = Math.Abs(turn * turningSpeed * terrainTurning * Math.Min(playerRB.velocity.magnitude / (2 * maxSpeed / 3), 1));
        turnSp = Math.Min(Math.Max(-maxTS, turnSp), maxTS);
        playerRB.rotation += turnSp;

        // if(ctrls.GetTurn() != 0 && turnSp != maxTS) Debug.Log("Turn: " + ctrls.GetTurn() + " turnSp: " + turnSp + " maxTS: " + maxTS );

        //Vector2 newVel = new Vector2();
        Vector2 accel = new Vector2();
        // We get the rotation, convert to radians, and also add 90 degrees (PI/2 radians) to get our direction angle.
        rotation = (float)(Math.PI / 180.0) * playerRB.rotation + (float)(Math.PI / 2.0);
        //new vel should be in the direction of rotation
        newVel.Set((float)Math.Cos(rotation), (float)Math.Sin(rotation));

        accel = newVel * acceleration * draftBoost * terrainSpeed * ctrls.GetSpeed();

        // Not moving, want to reverse
        if (playerRB.velocity.magnitude == 0 && ctrls.GetSpeed() < 0)
        {
            if (wait > 0)
            {
                wait--;
                newVel.Set(0, 0);
            }
            else
            {
                newVel = accel;
                wait = reverseWait;
            }
        }
        // Moving backward
        else if (!(Vector2.Angle(playerRB.velocity, newVel) < 90))
        {
            newVel = Vector2.ClampMagnitude((newVel * (-1) * playerRB.velocity.magnitude) + accel, maxReverse);
        }
        // Move forward
        else
        {
            newVel = Vector2.ClampMagnitude((newVel * playerRB.velocity.magnitude) + accel, maxSpeed);
            // Check for direction switch
            if (!(Vector2.Angle(playerRB.velocity, newVel) < 90))
            {
                newVel.Set(0, 0);
            }
        }

        playerRB.velocity = newVel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)))
        //if (collision.gameObject.tag == "Player")
        {
            drafting = true;
            Debug.Log("Entered Drafting");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)))
        // if (collision.gameObject.tag == "Player")
        {
            drafting = false;
            Debug.Log("Exited Drafting");
        }
    }
}