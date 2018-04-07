﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator charAnimator;

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
    /*Decay rate for velocity. vel = decay*vel */
    private float decayRate = 0.97f;
    /* Decay rate for misc forces */
    private float forceDecayRate = 0.75f;
    private float turnIncr;
    private float turningSpeed;
    private float maxTS;
    private float turnSp = 0;

    private float acceleration;
    private float maxSpeed;
    private float maxReverse;

    private float[] speedList = new float [7];
    public enum BOOSTS { STANDARD, STANDARD_BACK, PAD, PAD_BACK, DRAFT, DRIFT, POWERUP };
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
    
    private bool finished = false;
    private int ghostLayer = 13;

    private Coroutine lastIncapCoroutine = null;
    private Coroutine lastBoostFCoroutine = null;
    private Coroutine lastBoostBCoroutine = null;

    private Component[] boxes;
    private Component[] caps;
    /*Speed for the last update */
    private float lastSpeed = 0;
    /* Current speed */    
    private float speed = 0;
    /*This vector will contain data about other accelerations; E.G. Bouncing off walls and players will set this vector*/
    private Vector2 miscForces;
    /* Might not be necessary, but I'm going to keep track of the last non-colliding positon, just in case*/
    private Vector2 lastKnownGoodPosition;

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
    private float GetTurnIncrement()
    {
        float turn;
        // Forward
        if (speed > 0)
        {
            turn = ctrls.GetTurn();
        }
        // Backward
        else
        {
            turn = -ctrls.GetTurn();
        }

        //Add rotation
        turnSp += turn * turnIncr;
        maxTS = Math.Abs(turn * turningSpeed * terrainTurning * Math.Min(speed / (2 * maxSpeed / 3), 1));
        turnSp = Math.Min(Math.Max(-maxTS, turnSp), maxTS);
        return turnSp;
    }

    // Turn player method for drifting (NEED TO ALLOW TURNING WHEN NOT MOVING)
    private float GetTurningIncrementDrifting()
    {
        float turn = ctrls.GetTurn();

        //Add rotation
        turnSp += turn * turnIncr;// * (playerRB.velocity.magnitude/(maxSpeed/2));
        maxTS = Math.Abs(turn * turningSpeed * terrainTurning /* Math.Min(playerRB.velocity.magnitude / (2 * maxSpeed / 3), 1)*/) * 1.5f;
        turnSp = Math.Min(Math.Max(-maxTS, turnSp), maxTS);
        return turnSp;
    }

    // Use this for initialization
    void Start()
    {
        initValues();
        animator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        ctrls = Inputs.GetController(playerNumber);
        playerRB.freezeRotation = true;
        col = GetComponentInChildren<CapsuleCollider2D>();
        draftingHitbox = GetComponent<BoxCollider2D>();

        wait = reverseWait;

        speedList[(int) BOOSTS.STANDARD] = maxSpeed;
        speedList[(int) BOOSTS.STANDARD_BACK] = maxReverse;
        speedList[(int) BOOSTS.PAD] = 1.75f;
        speedList[(int) BOOSTS.PAD_BACK] = 1f;
        speedList[(int) BOOSTS.DRAFT] = 1.66f;
        speedList[(int) BOOSTS.DRIFT] = 1.66f;
        speedList[(int) BOOSTS.POWERUP] = 1.75f;


        //Debug.Log(PlayerData.playerChars[playerNumber - 1] < 0);
        powerups = new Powerup[(int)POWERUP_DIRECTION.SIZE];
        //instantiate a powerup
        powerups[(int)POWERUP_DIRECTION.UP] = powerupInstatiatior.GetPowerup(PowerupType.CHICKEN, this);
        powerups[(int)POWERUP_DIRECTION.DOWN] = powerupInstatiatior.GetPowerup(PowerupType.PUFFERFISH, this);
        powerups[(int)POWERUP_DIRECTION.LEFT] = powerupInstatiatior.GetPowerup(PowerupType.SQUID, this);
        powerups[(int)POWERUP_DIRECTION.RIGHT] = powerupInstatiatior.GetPowerup(PowerupType.FROG, this);

        boxes = gameObject.GetComponentsInChildren<BoxCollider2D>();
        caps = gameObject.GetComponentsInChildren<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float accel = 0;
        float turningAccel = 0;
        //Debug.Log("Idle " + playerNumber + " " + ctrls.GetSpeed());
        switch (state)
        {
            case STATES.COUNTDOWN:
                if (!mapEvents.inCountdown()) state = STATES.IDLE;

                break;

            //Idle state
            case STATES.IDLE:
                speed = 0;
                //checking change states
                if (ctrls.GetSpeed() < 0)
                {
                    state = STATES.MOVE_B;
                   
                }
                else if (ctrls.GetSpeed() > 0)
                {
                    state = STATES.MOVE_F;
                   
                }

                break;

            //Moving forward state
            case STATES.MOVE_F:
                //Debug.Log("MOVING F");
                //if (playerNumber == 2) Debug.Log("In Move Forward.");
                accel = acceleration * ctrls.GetSpeed();
                turningAccel = GetTurnIncrement();

                if (ctrls.GetSpeed() <= 0)
                {
                    state = STATES.DECEL;
                   
                }
                if (drafting && GetSpeedPercent() > 0.5)
                {
                    state = STATES.DRAFT;
                }
                if (ctrls.GetA())
                {
                    driftDir = playerRB.rotation;
                    state = STATES.DRIFT;
                   
                }

                break;


            case STATES.MOVE_B:

                //if (playerNumber == 2) Debug.Log("In Move Backward.");

                accel = acceleration * ctrls.GetSpeed();
                turningAccel = GetTurnIncrement();

                if (ctrls.GetSpeed() >= 0)
                {
                    state = STATES.ACCEL;
                }

                break;

            case STATES.ACCEL:

                //if (playerNumber == 2) Debug.Log("In Accel.");

                //Debug.Log("Before newVel " + newVel);
               // Debug.Log("Velocity " + playerRB.velocity);

                accel = acceleration * ctrls.GetSpeed();
                turningAccel = GetTurnIncrement();
                //Debug.Log("After newVel " + newVel);

                if (ctrls.GetSpeed() < 0)
                {
                    state = STATES.MOVE_B;
                }
                if (speed > 0)
                {
                    state = STATES.IDLE;
                }
                break;
            case STATES.DECEL:

                //if (playerNumber == 1) Debug.Log("In Decel");
        
                accel = acceleration * ctrls.GetSpeed();
                turningAccel = GetTurnIncrement();

                if (ctrls.GetSpeed() > 0)
                {
                    state = STATES.MOVE_F;
                }
                /*TODO make sure this check is correct; */
                if (speed <= 0.01)
                {
                    state = STATES.STOP_B;
                }

                break;
            case STATES.STOP_B:
                speed = 0;
                //if (playerNumber == 1) Debug.Log("In Stopped");

                if (wait > 0)
                {
                    //if (playerNumber == 1) Debug.Log("Waiting");
                    wait--;
                }
                else
                {
                    wait = reverseWait;
                    state = STATES.IDLE;
                }

                if (ctrls.GetSpeed() > 0)
                {
                    state = STATES.MOVE_F;
                }


                break;
            case STATES.OILED:
                turningAccel = GetTurningIncrementDrifting();

                if (speed < 50)
                {
                    state = STATES.MOVE_F;
                }
                break;
            case STATES.DRIFT:

                driftTime++;
                turningAccel = GetTurningIncrementDrifting();
                if ((!ctrls.GetA() && driftTime > 80) || driftTime > 300)
                {
                    StartBoost(BOOSTS.DRIFT, driftTime * Time.fixedDeltaTime);
                    driftTime = 0;
                    
                }
                else if (!ctrls.GetA() && driftTime <= 100)
                {
                    driftTime = 0;
                    state = STATES.MOVE_F;
                }
                break;
            case STATES.DRAFT:

                draftTime++;
                if (draftTime > 125) drafting = false;

                accel = acceleration * ctrls.GetSpeed();
                turningAccel = GetTurnIncrement();

                if (!drafting && draftTime > 0)
                {
                    StartBoost(BOOSTS.DRAFT, 2 * draftTime * Time.fixedDeltaTime);
                    draftTime = 0;
                }
                if (ctrls.GetSpeed() <= 0)
                {
                    draftTime = 0;
                    state = STATES.DECEL;
                }

                break;

            case STATES.BOOST:
                /* TODO don't hardcode values */
                accel = 100000;
                turningAccel = GetTurnIncrement();

                if (ctrls.GetA())
                {
                    StopCoroutine(lastBoostFCoroutine);
                    lastBoostFCoroutine = null;
                    maxSpeed = speedList[(int)BOOSTS.STANDARD];
                    driftDir = playerRB.rotation;
                    state = STATES.DRIFT;
                }

                break;

            case STATES.BOOST_B:
                accel = -100000;
                turningAccel = GetTurnIncrement();

                break;

            case STATES.INCAPACITATED:
                //TODO: THIS COROUTINE IS CREATED EVERY GAME TICK!!!
                //Play a player getting electrocuted sound and/or animation.               
                //TODO right now, decay happens at a uniform rate. We could change the velocity decay rate here,
                //then change it back when we leave this state.
                break;

            case STATES.FLYING:

                //set new velocity             
                accel =  speedList[(int)BOOSTS.STANDARD] * 1.5f;
                turningAccel = GetTurnIncrement();

                break;

        }

        if (state != STATES.COUNTDOWN || !finished)
        {
            DoPowerups();
        }

        DoPhysics(accel, turningAccel);

        animator.SetFloat("TurnVal", -ctrls.GetTurn());
        charAnimator.SetFloat("TurnVal", -ctrls.GetTurn());
        float animspeed = speed;
        if (state == STATES.MOVE_B || state == STATES.ACCEL) animspeed = -animspeed;
        animator.SetFloat("Speed", animspeed);
        charAnimator.SetFloat("Speed", animspeed);
    }

    public void stopLastIncapCoroutine()
    {
        StopCoroutine(lastIncapCoroutine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerWallCollider")
        {
            drafting = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerWallCollider")
        // if (collision.gameObject.tag == "Player")
        {
            drafting = false;
        }
    }

    
    public float GetSpeedPercent()
    {
        return playerRB.velocity.magnitude / speedList[(int)BOOSTS.STANDARD];
    }


    public void StartBoost(BOOSTS b, float btime)
    {
        state = STATES.BOOST;
        maxSpeed = speedList[(int)b];
        maxReverse = speedList[(int)BOOSTS.STANDARD_BACK];

        //overwrite the old coroutine if one exists
        if (lastBoostFCoroutine != null)
            StopCoroutine(lastBoostFCoroutine);
        lastBoostFCoroutine = StartCoroutine(endBoost(btime));
    }

    public void StartBoostB(BOOSTS b, float btime)
    {
        state = STATES.BOOST_B;
        maxSpeed = speedList[(int)BOOSTS.STANDARD];
        maxReverse = speedList[(int)b];

        //overwrite the old coroutine if one exists
        if (lastBoostBCoroutine != null)
            StopCoroutine(lastBoostBCoroutine);
        lastBoostBCoroutine = StartCoroutine(endBoostB(btime));
    }

    IEnumerator endBoost(float time)
    {
        yield return new WaitForSeconds(time);
        maxSpeed = speedList[(int) BOOSTS.STANDARD];
        state = STATES.MOVE_F;
        lastBoostFCoroutine = null;
    }
    IEnumerator endBoostB(float time)
    {
        yield return new WaitForSeconds(time);
        maxReverse = speedList[(int) BOOSTS.STANDARD_BACK];
        state = STATES.MOVE_B;
        lastBoostBCoroutine = null;
    }

    public void StartIncap(float itime)
    {
        state = STATES.INCAPACITATED;
        maxSpeed = speedList[(int)BOOSTS.STANDARD];
        maxReverse = speedList[(int)BOOSTS.STANDARD_BACK];
        setGhosted(true);

        //Stop any boost coroutines
        if (lastBoostFCoroutine != null)
            StopCoroutine(lastBoostFCoroutine);
        if (lastBoostBCoroutine != null)
            StopCoroutine(lastBoostBCoroutine);

        //overwrite the old incap coroutine if one exists
        if (lastIncapCoroutine != null)
            StopCoroutine(lastIncapCoroutine);
        lastIncapCoroutine = StartCoroutine(endIncapacitated(itime));
    }

    IEnumerator endIncapacitated(float time)
    {
        yield return new WaitForSeconds(time);
        setGhosted(false);
        state = STATES.IDLE;
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
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RisenPlayer";      // Makes flyer render above other players
            charAnimator.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RisenPlayer";
        }
        else
        {
            state = STATES.MOVE_F;
            gameObject.layer = 0;                                                          // Return to default layer
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";         // Returns to initial order
            charAnimator.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        }
    }

    public STATES GetPlayerState()
    {
        return state;
    }
    

    public void setDriftDir(float DriftDirection)
    {
        driftDir = DriftDirection;
    }


    public void SetTransparency(bool b)
    {
        if (b)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            charAnimator.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            charAnimator.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void setGhosted(bool set)
    {
        if (set)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].gameObject.layer = ghostLayer;
            }
            for (int i = 0; i < caps.Length; i++)
            {
                caps[i].gameObject.layer = ghostLayer;
            }

            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RisenPlayer";                  // Makes flyer render above other players
            charAnimator.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RisenPlayer";

            SetTransparency(true);
        }
        else
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].gameObject.layer = 0;
            }
            for (int i = 0; i < caps.Length; i++)
            {
                caps[i].gameObject.layer = 0;
            }

            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";                  // Makes flyer render above other players
            charAnimator.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";

            SetTransparency(false);
        }
    }

    public void SetFinished()
    {
        finished = true;
        setGhosted(true);
    }

    /* NEW PHYSICS STUFF */
    private void DoPhysics(float acceleration, float turnIncrement){
        /* TODO calculate the worldspace coodinates for these rotations!*/
        Vector3 calculatedVelocity = new Vector3(Mathf.Cos(Mathf.Deg2Rad*(transform.eulerAngles.z + turnIncrement + 90)), 
                                                 Mathf.Sin(Mathf.Deg2Rad*(transform.eulerAngles.z + turnIncrement + 90)), 0f);
        //if(turnIncrement != 0) Debug.Log("TurnIncrement = " + turnIncrement);
        lastSpeed = speed;
        /*increase speed by acceleration */
        speed +=  acceleration;
        speed = Mathf.Clamp(speed, -maxReverse, maxSpeed);
        //Debug.Log("Speed: " + speed);
        calculatedVelocity *= speed;
        //We add acceleration to velocity, cap it to the max speed, and then calculate the next position
        calculatedVelocity += (Vector3)miscForces;
        //Decay velocity. This decay should be proportional to the velocity.
        //In real physics, we would be propotional to the square of velocity. Here we'll try a linear relationship.
        speed *= decayRate;
        miscForces *= forceDecayRate;
        //Calculate the position, as god intended
        transform.position += calculatedVelocity;
        transform.Rotate(0, 0, turnIncrement);
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.collider.tag == "PlayerDiamondCollider"){
            Player otherPlayer = other.collider.GetComponentInParent<Player>();
            /* TODO scale this by some number or something */
            miscForces += (Vector2)(transform.position - otherPlayer.transform.position);
        }else{
            /* TODO make it so that we KNOW we are colliding with a wall
                Also, again, could use some scaling 
            */
            miscForces += (Vector2)(other.contacts[0].normal);
        }
    }
}
