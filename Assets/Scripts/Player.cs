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

    public enum STATES { IDLE, MOVE_F, MOVE_B, DECEL, ACCEL, STOP_B, DRIFT, DRAFT, BOOST, BOOST_B, COUNTDOWN, INCAPACITATED, OILED };
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

    private bool isFlying = false;

    private Coroutine lastIncapCoroutine = null;
    private Coroutine lastBoostFCoroutine = null;
    private Coroutine lastBoostBCoroutine = null;


 

    

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
        speedList[(int) BOOSTS.STANDARD_BACK] = maxReverse;
        speedList[(int) BOOSTS.PAD] = 200;
        speedList[(int) BOOSTS.PAD_BACK] = 120;
        speedList[(int) BOOSTS.DRAFT] = 175;
        speedList[(int) BOOSTS.DRIFT] = 175;
        speedList[(int) BOOSTS.POWERUP] = 200;


        //Debug.Log(PlayerData.playerChars[playerNumber - 1] < 0);
        powerups = new Powerup[(int)POWERUP_DIRECTION.SIZE];
        //instantiate a powerup
        powerups[(int)POWERUP_DIRECTION.UP] = powerupInstatiatior.GetPowerup(PowerupType.CHICKEN, this);
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
                if (drafting && playerRB.velocity.magnitude > (maxSpeed / 2))
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
                    StartBoost(BOOSTS.DRIFT, driftTime * Time.fixedDeltaTime);
                    driftTime = 0;
                    
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

                break;

            case STATES.BOOST_B:
                //setting newvel direction at unit length
                setNewVelRotation(ref newVel);
                //change player turning
                setRotation(newVel);
                //setting newvel direction to turning direction
                setNewVelRotation(ref newVel);
                accel = newVel * acceleration;          
                //set new velocity             
                newVel = Vector2.ClampMagnitude((newVel * -100000) + accel, maxReverse);

                break;

            case STATES.INCAPACITATED:
                //TODO: THIS COROUTINE IS CREATED EVERY GAME TICK!!!
                //Play a player getting electrocuted sound and/or animation.               
                
                newVel = newVel * playerRB.velocity.magnitude * 0.9f;

                break;
        }

        playerRB.velocity = newVel;

        if (state != STATES.COUNTDOWN)
        {
            DoPowerups();
        }







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
    public void stopLastIncapCoroutine()
    {
        StopCoroutine(lastIncapCoroutine);
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


    public void StartBoost(BOOSTS b, float btime)
    {
        Debug.Log("STARTING NEW BOOST");
        animator.SetTrigger("Boosted");
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
        Debug.Log("STOPPING BOOST");
        maxSpeed = speedList[(int) BOOSTS.STANDARD];
        animator.SetTrigger("ToMoveF");
        state = STATES.MOVE_F;
        lastBoostFCoroutine = null;
        Debug.Log("RESET MAX SPEED");
    }
    IEnumerator endBoostB(float time)
    {
        yield return new WaitForSeconds(time);
        maxReverse = speedList[(int) BOOSTS.STANDARD_BACK];
        state = STATES.MOVE_B;
        lastBoostBCoroutine = null;
        Debug.Log("RESET MAX SPEED");
    }

    public void StartIncap(float itime)
    {
        state = STATES.INCAPACITATED;
        maxSpeed = speedList[(int)BOOSTS.STANDARD];
        maxReverse = speedList[(int)BOOSTS.STANDARD_BACK];

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
        state = STATES.IDLE;
        Debug.Log("SENT TO IDLE");
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
    public void SetIsFlying(bool b)
    {
        isFlying = b;
    }


    // Get boolean status of Eagle powerup
    public bool GetIsFlying()
    {
        return isFlying;
    }

    public void setDriftDir(float DriftDirection)
    {
        driftDir = DriftDirection;
    }
}
