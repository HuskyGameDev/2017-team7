using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private enum STATES { IDLE, MOVE_F, MOVE_B, DECEL, ACCEL, STOP_B, DRIFT, DRAFT, BOOST };
    private STATES state = STATES.IDLE;

    private Rigidbody2D playerRB;

    private Controller ctrls;

    public float turnIncr;
    public float turningSpeed;
    private float maxTS;
    private float turnSp = 0;

    public float acceleration;
    public float maxSpeed;
    public float maxReverse = 50;

    private CapsuleCollider2D collider;
    private BoxCollider2D draftingHitbox;
    private bool drafting = false;
    private int draftTime = 0;
    private float draftBoost = 1;
     

    private bool drifting;
    private float tempRotation;



    public int playerNumber;

    public PhysicsMaterial2D wallMaterial, playerMaterial;

    private int reverseWait = 10;
    private int wait;

    public float terrainSpeed = 1;
    public float terrainTurning = 1;



    private void checkDrafting()
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

    }

    //passed in unit length vector in direction we are facing prior to calculating rotation
    private void setRotation(Vector2 newVel)
    {
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
    }


    private void setNewVelRotation(Vector2 newVel)
    {
        float rotation = Mathf.Deg2Rad * (playerRB.rotation + 90);
        newVel.Set((float)Math.Cos(rotation), (float)Math.Sin(rotation));
    }

    // Use this for initialization
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        ctrls = Inputs.GetController(playerNumber);
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

        Vector2 newVel = new Vector2();

        Vector2 accel = new Vector2();


        switch (state)
        {

            //Idle state
            case STATES.IDLE:

                newVel.Set(0, 0);

                Debug.Log("Idle " + playerNumber + " " + ctrls.GetLT());

                //checking change states
                if (ctrls.GetSpeed() < 0) state = STATES.MOVE_B;
                else if (ctrls.GetSpeed() > 0) state = STATES.MOVE_F;

                break;

            //Moving forward state
            case STATES.MOVE_F:

                //setting newvel direction at unit length
                setNewVelRotation(newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(newVel);

                accel = newVel * acceleration * ctrls.GetSpeed();

                //set new velocity             
                newVel = Vector2.ClampMagnitude((newVel * playerRB.velocity.magnitude) + accel, maxSpeed);

                if (ctrls.GetSpeed() <= 0) state = STATES.DECEL;

                break;


            case STATES.MOVE_B:

                //setting newvel direction at unit length
                setNewVelRotation(newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(newVel);

                accel = newVel * acceleration * ctrls.GetSpeed();

                //set new velocity             
                newVel = Vector2.ClampMagnitude((newVel * (-1) * playerRB.velocity.magnitude) + accel, maxReverse);

                if (ctrls.GetSpeed() >= 0) state = STATES.ACCEL;

                break;

            case STATES.ACCEL:

                //setting newvel direction at unit length
                setNewVelRotation(newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(newVel);

                if (ctrls.GetSpeed() == 0) newVel *= playerRB.velocity.magnitude * 0.95f;
                else
                {
                    accel = newVel * acceleration * ctrls.GetSpeed();
                    //set new velocity             
                    newVel = (newVel * (-1) * playerRB.velocity.magnitude) + accel;
                }

                if (ctrls.GetSpeed() < 0) state = STATES.MOVE_B;
                if (!(Vector2.Angle(playerRB.velocity, newVel) < 90) || newVel.magnitude < 0.05) state = STATES.IDLE;
                break;
            case STATES.DECEL:
                //setting newvel direction at unit length
                setNewVelRotation(newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(newVel);
                
                //setting player speed to slightly smaller ratio of current velocity
                if (ctrls.GetSpeed() == 0) newVel *= playerRB.velocity.magnitude * 0.95f;
                else
                {
                    accel = newVel * acceleration * ctrls.GetSpeed();
                    //set new velocity             
                    newVel = (newVel * playerRB.velocity.magnitude) + accel;
                }

                if (ctrls.GetSpeed() > 0) state = STATES.MOVE_F;
                if (!(Vector2.Angle(playerRB.velocity, newVel) < 90) || newVel.magnitude < 0.05) state = STATES.STOP_B;

                break;
            case STATES.STOP_B:

                newVel.Set(0, 0);

                if (wait > 0)
                {
                    wait--;
                }
                else
                {
                    //newVel = accel;
                    wait = reverseWait;
                    state = STATES.IDLE;
                }

                if (ctrls.GetSpeed() > 0) state = STATES.MOVE_F;

                break;
            case STATES.DRIFT: break;
            case STATES.DRAFT: break;
            case STATES.BOOST: break;
        }

        playerRB.velocity = newVel;


        //private enum STATES { IDLE, MOVE_F, MOVE_B, DECEL_B, STOP_B, DRIFT, DRAFT, BOOST };







        //checkDrafting();
       
       // setRotation(newVel);

        // if(ctrls.GetTurn() != 0 && turnSp != maxTS) Debug.Log("Turn: " + ctrls.GetTurn() + " turnSp: " + turnSp + " maxTS: " + maxTS );


        //Vector2 accel = new Vector2();
        
       // rotation = Mathf.Deg2Rad * (playerRB.rotation + 90);
        
        //newVel.Set((float)Math.Cos(rotation), (float)Math.Sin(rotation));

        //accel = newVel * acceleration * draftBoost * terrainSpeed * ctrls.GetSpeed();

        // Moving backward
        //else if (!(Vector2.Angle(playerRB.velocity, newVel) < 90))
        //{
        //    newVel = Vector2.ClampMagnitude((newVel * (-1) * playerRB.velocity.magnitude) + accel, maxReverse);
        //}
        // Move forward
        //else
        //{
        //    newVel = Vector2.ClampMagnitude((newVel * playerRB.velocity.magnitude) + accel, maxSpeed);
        //    // Check for direction switch
        //    if (!(Vector2.Angle(playerRB.velocity, newVel) < 90))
        //    {
        //        newVel.Set(0, 0);
        //    }
        //}
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