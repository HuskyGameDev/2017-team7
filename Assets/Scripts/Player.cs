using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CameraControl overheadCamera;

    public Players players;

    [System.NonSerialized]
    public Animator animator;

    public enum STATES { IDLE, MOVE_F, MOVE_B, DECEL, ACCEL, STOP_B, DRIFT, DRAFT, BOOST, BOOST_B, COUNTDOWN, INCAPACITATED, OILED, FLYING };
    public STATES state = STATES.COUNTDOWN;

    [System.NonSerialized]
    public Rigidbody2D playerRB;

    public Coroutine boostingCoR;

    private Controller ctrls;

    private float turnIncr;
    private float turningSpeed;
    private float maxTS;
    private float turnSp = 0;

    private float acceleration;
    private float maxSpeed;
    private float maxReverse;

    private float[] speedList = new float [4];
    public enum BOOSTS { STANDARD, BOOST_PAD, DRAFT_BOOST, DRIFT_BOOST };
    private BOOSTS boost = BOOSTS.STANDARD;
    private float boostTime;

    private CapsuleCollider2D col;
    private BoxCollider2D draftingHitbox;
    private bool drafting = false;
    private int draftTime = 0;
    private float draftBoost = 1;

    private float driftDir;
    private int driftTime = 0;

    private bool drifting;
    private float tempRotation;

    public int playerNumber;

    private PhysicsMaterial2D wallMaterial, playerMaterial;

    private int reverseWait = 10;
    private int wait;

    [System.NonSerialized]
    public float terrainSpeed = 1;
    [System.NonSerialized]
    public float terrainTurning = 1;

    public AudioSource engineSound;
    public AudioClip loopingEngine;

    private Map mapEvents;

    private enum POWERUP_DIRECTION {UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3, SIZE = 4}
    private PowerupInstantiator powerupInstatiatior;
    private Powerup[] powerups;

    private bool isFlying = false;


    IEnumerator endBoost(float time)
    {
        yield return new WaitForSeconds(time);
        maxSpeed = speedList[(int)boost];
        animator.SetTrigger("ToMoveF");
        Debug.Log("RESET MAX SPEED");
    }
    IEnumerator endBoostB()
    {
        yield return new WaitForSeconds(1);
        maxReverse = 60;
        Debug.Log("RESET MAX SPEED");

    }

    IEnumerator endIncapacitated(float time) {
        yield return new WaitForSeconds(time);
        state = STATES.IDLE;
        Debug.Log("SENT TO IDLE");
    }

    private void initValues()
    {
        turnIncr = players.turnIncr;
        turningSpeed = players.turningSpeed;
        acceleration = players.acceleration;
        maxSpeed = players.maxSpeed;
        maxReverse = players.maxReverse;
        wallMaterial = players.wallMaterial;
        playerMaterial = players.playerMaterial;
        terrainSpeed = players.terrainSpeed;
        terrainTurning = players.terrainTurning;
        mapEvents = players.mapEvents;
        powerupInstatiatior = players.powerupInstantiator;
    }


    private void checkDrafting()
    {
        if (drafting && GetSpeedPercent() > 0.5)
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


    private void setNewVelRotation(ref Vector2 newVel)
    {
        float rotation = Mathf.Deg2Rad * (playerRB.rotation + 90);
        newVel.Set((float)Math.Cos(rotation), (float)Math.Sin(rotation));
    }

    // Turn player method for drifting (NEED TO ALLOW TURNING WHEN NOT MOVING)
    private void setRotationDrifting()
    {
        float turn = ctrls.GetTurn();

        //Add rotation
        turnSp += turn * turnIncr;// * (playerRB.velocity.magnitude/(maxSpeed/2));
        maxTS = Math.Abs(turn * turningSpeed * terrainTurning /* Math.Min(playerRB.velocity.magnitude / (2 * maxSpeed / 3), 1)*/) * 1.5f;
        turnSp = Math.Min(Math.Max(-maxTS, turnSp), maxTS);
        playerRB.rotation += turnSp;
    }


    private void setNewVelRotationDrifting(ref Vector2 newVel)
    {
        float rotation = Mathf.Deg2Rad * (driftDir + 90);
        newVel.Set((float)Math.Cos(rotation), (float)Math.Sin(rotation));
    }

    // Use this for initialization
    void Start()
    {
        initValues();
        animator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        ctrls = Inputs.GetController(playerNumber);
        playerRB.freezeRotation = true;
        col = GetComponent<CapsuleCollider2D>();
        draftingHitbox = GetComponent<BoxCollider2D>();

        speedList[(int) BOOSTS.STANDARD] = maxSpeed;
        speedList[(int) BOOSTS.BOOST_PAD] = 200;
        speedList[(int) BOOSTS.DRAFT_BOOST] = 175;
        speedList[(int) BOOSTS.DRIFT_BOOST] = 175;
        
        
        //Debug.Log(PlayerData.playerChars[playerNumber - 1] < 0);
        powerups = new Powerup[(int)POWERUP_DIRECTION.SIZE];
        //instantiate a powerup
        powerups[(int)POWERUP_DIRECTION.UP] = powerupInstatiatior.GetPowerup(PowerupType.EAGLE, this);
        powerups[(int)POWERUP_DIRECTION.DOWN] = powerupInstatiatior.GetPowerup(PowerupType.EEL, this);
        powerups[(int)POWERUP_DIRECTION.LEFT] = powerupInstatiatior.GetPowerup(PowerupType.SQUID, this);
        powerups[(int)POWERUP_DIRECTION.RIGHT] = powerupInstatiatior.GetPowerup(PowerupType.FROG, this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        animator.SetFloat("TurnVal", -ctrls.GetTurn());
        //Debug.Log(ctrls.GetTurn());
        Vector2 newVel = new Vector2();

        Vector2 accel = new Vector2();

        //Debug.Log("Idle " + playerNumber + " " + ctrls.GetSpeed());
        switch (state)
        {

            case STATES.COUNTDOWN:
                if (!mapEvents.inCountdown()) state = STATES.IDLE;

                break;

            //Idle state
            case STATES.IDLE:

                //if (playerNumber == 2) Debug.Log("In Idle.");
                newVel.Set(0, 0);



                //checking change states
                if (ctrls.GetSpeed() < 0)
                {
                    state = STATES.MOVE_B;
                    animator.SetTrigger("LT");
                }
                else if (ctrls.GetSpeed() > 0)
                {
                    state = STATES.MOVE_F;
                    animator.SetTrigger("RT");
                }

                break;

            //Moving forward state
            case STATES.MOVE_F:
                //Debug.Log("MOVING F");
                //if (playerNumber == 2) Debug.Log("In Move Forward.");


                //setting newvel direction at unit length
                setNewVelRotation(ref newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(ref newVel);

                accel = newVel * acceleration * ctrls.GetSpeed();

                //set new velocity             
                newVel = Vector2.ClampMagnitude((newVel * playerRB.velocity.magnitude) + accel, maxSpeed * terrainSpeed);

                if (ctrls.GetSpeed() <= 0)
                {
                    state = STATES.DECEL;
                    animator.SetTrigger("NT");
                }
                if (drafting && GetSpeedPercent() > 0.5)
                {
                    state = STATES.DRAFT;
                }
                if (ctrls.GetB())
                {
                    driftDir = playerRB.rotation;
                    state = STATES.DRIFT;
                    animator.SetTrigger("ToDrift");
                }

                break;


            case STATES.MOVE_B:

                //if (playerNumber == 2) Debug.Log("In Move Backward.");

                //setting newvel direction at unit length
                setNewVelRotation(ref newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(ref newVel);

                accel = newVel * acceleration * ctrls.GetSpeed();

                //set new velocity             
                newVel = Vector2.ClampMagnitude((newVel * (-1) * playerRB.velocity.magnitude) + accel, maxReverse * terrainSpeed);

                if (ctrls.GetSpeed() >= 0)
                {
                    state = STATES.ACCEL;
                    animator.SetTrigger("NT");
                }

                break;

            case STATES.ACCEL:

                //if (playerNumber == 2) Debug.Log("In Accel.");

                //setting newvel direction at unit length
                setNewVelRotation(ref newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(ref newVel);

                //Debug.Log("Before newVel " + newVel);
               // Debug.Log("Velocity " + playerRB.velocity);

                if (ctrls.GetSpeed() == 0) newVel *= -playerRB.velocity.magnitude * 0.99f;
                else
                {
                    accel = newVel * acceleration * ctrls.GetSpeed();
                   // Debug.Log("Accel " + accel);
                    //set new velocity             
                    newVel = (newVel * (-1) * playerRB.velocity.magnitude) + accel;
                }
                //Debug.Log("After newVel " + newVel);

                if (ctrls.GetSpeed() < 0)
                {
                    state = STATES.MOVE_B;
                    animator.SetTrigger("LT");
                }
                if (!(Vector2.Angle(playerRB.velocity, newVel) < 90) || newVel.magnitude < 0.05)
                {
                    state = STATES.IDLE;
                    animator.SetTrigger("ToIdle");
                }
                break;
            case STATES.DECEL:

                //if (playerNumber == 1) Debug.Log("In Decel");

                //setting newvel direction at unit length
                setNewVelRotation(ref newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(ref newVel);
                
                //setting player speed to slightly smaller ratio of current velocity
                if (ctrls.GetSpeed() == 0) newVel *= playerRB.velocity.magnitude * 0.99f;
                else
                {
                    accel = newVel * acceleration * ctrls.GetSpeed();
                    //set new velocity             
                    newVel = (newVel * playerRB.velocity.magnitude) + accel;
                    //Debug.Log("New vel: " + newVel + " Accel: " + accel);
                }

                if (ctrls.GetSpeed() > 0)
                {
                    state = STATES.MOVE_F;
                    animator.SetTrigger("RT");
                }
                //Debug.Log("NewVel mag: " + newVel.magnitude);
                //Debug.Log("NewVel angle: " + Vector2.Angle(playerRB.velocity, newVel));

                if (!(Vector2.Angle(playerRB.velocity, newVel) < 90) || newVel.magnitude < 1.0)
                {
                    state = STATES.STOP_B;
                    animator.SetTrigger("ToStop");
                }

                break;
            case STATES.STOP_B:

                //if (playerNumber == 2) Debug.Log("In Stopped");

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
                    animator.SetTrigger("ToIdle");
                }

                if (ctrls.GetSpeed() > 0)
                {
                    state = STATES.MOVE_F;
                    animator.SetTrigger("RT");
                }


                break;
            case STATES.OILED:
                
                //setting newvel direction at unit length
                setNewVelRotationDrifting(ref newVel);
                //change player turning
                setRotationDrifting();

                newVel = newVel * playerRB.velocity.magnitude * 0.99f;

                if (playerRB.velocity.magnitude < 50)
                {
                    state = STATES.MOVE_F;
                    animator.SetTrigger("ToMoveF");
                }
                break;
            case STATES.DRIFT:

                driftTime++;

                //setting newvel direction at unit length
                setNewVelRotationDrifting(ref newVel);
                //change player turning
                setRotationDrifting();

                //set new velocity             
                newVel = newVel * playerRB.velocity.magnitude * 0.99f;

                if ((!ctrls.GetB() && driftTime > 80) || driftTime > 300)
                {
                    SetBoost(BOOSTS.DRIFT_BOOST, driftTime * Time.fixedDeltaTime);
                    driftTime = 0;
                    state = STATES.BOOST;
                    animator.SetTrigger("Boosted");
                }
                else if (!ctrls.GetB() && driftTime <= 100)
                {
                    driftTime = 0;
                    state = STATES.MOVE_F;
                    animator.SetTrigger("ToMoveF");
                }
                break;
            case STATES.DRAFT:

                draftTime++;
                if (draftTime > 125) drafting = false;

                //setting newvel direction at unit length
                setNewVelRotation(ref newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(ref newVel);

                accel = newVel * acceleration * ctrls.GetSpeed();

                //set new velocity             
                newVel = Vector2.ClampMagnitude((newVel * playerRB.velocity.magnitude) + accel, maxSpeed * terrainSpeed);

                if (!drafting && draftTime > 0)
                {
                    SetBoost(BOOSTS.DRAFT_BOOST, 2 * draftTime * Time.fixedDeltaTime);
                    draftTime = 0;
                    state = STATES.BOOST;
                }
                if (ctrls.GetSpeed() <= 0)
                {
                    draftTime = 0;
                    state = STATES.DECEL;
                }

                break;

            case STATES.BOOST:

                //Debug.Log("BOOSTING");
                
                maxSpeed = speedList[(int)boost];
                maxReverse = 60;

                //setting newvel direction at unit length
                setNewVelRotation(ref newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(ref newVel);

                accel = newVel * acceleration;

                //set new velocity             
                newVel = Vector2.ClampMagnitude((newVel * 100000) + accel, maxSpeed);
                Debug.Log(boostTime);
                //if (ctrls.GetSpeed() <= 0) state = STATES.DECEL;
                StopAllCoroutines();
                StartCoroutine(endBoost(boostTime));

                // Reset states
                state = STATES.MOVE_F;
                boost = BOOSTS.STANDARD;


                break;
            case STATES.BOOST_B:

                //Debug.Log("BOOSTING");
                maxSpeed = speedList[(int)boost];
                maxReverse = 120;

                //setting newvel direction at unit length
                setNewVelRotation(ref newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(ref newVel);

                accel = newVel * acceleration;

                //set new velocity             
                newVel = Vector2.ClampMagnitude((newVel * -100000) + accel, maxSpeed);

                //if (ctrls.GetSpeed() <= 0) state = STATES.DECEL;
                StopAllCoroutines();
                StartCoroutine(endBoostB());
                state = STATES.MOVE_B;
                boost = BOOSTS.STANDARD;
                break;

            case STATES.INCAPACITATED:

                //Play a player getting electrocuted sound and/or animation.

                StartCoroutine(endIncapacitated(1f));
                newVel = newVel * playerRB.velocity.magnitude * 0.9f;
                Debug.Log("INCAPACITATED");
                break;

            case STATES.FLYING:

                //setting newvel direction at unit length
                setNewVelRotation(ref newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(ref newVel);

                //accel = newVel * acceleration * ctrls.GetSpeed();

                //set new velocity             
                newVel = newVel * speedList[(int)BOOSTS.STANDARD] * 1.5f;

                

                break;
        }

        playerRB.velocity = newVel;

        if (state != STATES.COUNTDOWN)
        {
            DoPowerups();
        }

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

    
    public float GetSpeedPercent()
    {
        return playerRB.velocity.magnitude / speedList[(int)BOOSTS.STANDARD];
    }


    public void SetBoost(BOOSTS b, float btime)
    {
        boost = b;
        boostTime = btime;
    }

    public void DoPowerups(){
        if(powerups[(int)POWERUP_DIRECTION.UP] != null && ctrls.GetDPadUp()){
            powerups[(int)POWERUP_DIRECTION.UP].UsePowerup();
        }else if(powerups[(int)POWERUP_DIRECTION.RIGHT] != null && ctrls.GetDPadRight()){
            powerups[(int)POWERUP_DIRECTION.RIGHT].UsePowerup();
        }else if(powerups[(int)POWERUP_DIRECTION.DOWN] != null && ctrls.GetDPadDown()){
            powerups[(int)POWERUP_DIRECTION.DOWN].UsePowerup();
        }else if(powerups[(int)POWERUP_DIRECTION.LEFT] != null && ctrls.GetDPadLeft()){
            powerups[(int)POWERUP_DIRECTION.LEFT].UsePowerup();
        }
    }


    // Set boolean to show that Eagle powerup is in use
    public void SetPlayerStateFlying(bool b)
    {
        if (b == true)
        {
            state = STATES.FLYING;
            gameObject.layer = EaglePowerup.flyingLayer;
        }
        else
        {
            state = STATES.MOVE_F;
            gameObject.layer = 0;               // Return to default layer
        }
    }


    

    public void setDriftDir(float DriftDirection)
    {
        driftDir = DriftDirection;
    }
}
