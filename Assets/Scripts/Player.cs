using System;
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

    private float minDriftTime;
    private float maxDriftTime;

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

    private void initValues()
    {
        turnIncr = players.turnIncr;
        turningSpeed = players.turningSpeed;
        acceleration = players.acceleration;
        maxSpeed = players.maxSpeed;
        maxReverse = players.maxReverse;
        minDriftTime = players.minDriftTime * (1 / Time.fixedDeltaTime);
        maxDriftTime = players.maxDriftTime * (1 / Time.fixedDeltaTime);
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
        col = GetComponentInChildren<CapsuleCollider2D>();
        draftingHitbox = GetComponent<BoxCollider2D>();

        speedList[(int) BOOSTS.STANDARD] = maxSpeed;
        speedList[(int) BOOSTS.STANDARD_BACK] = maxReverse;
        speedList[(int) BOOSTS.PAD] = 325;
        speedList[(int) BOOSTS.PAD_BACK] = 120;
        speedList[(int) BOOSTS.DRAFT] = 175;
        speedList[(int) BOOSTS.DRIFT] = 250;
        speedList[(int) BOOSTS.POWERUP] = 200;


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

                if (ctrls.GetSpeed() == 0) newVel *= -playerRB.velocity.magnitude * 0.999f;
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
                }
                if (!(Vector2.Angle(playerRB.velocity, newVel) < 90) || newVel.magnitude < 0.05)
                {
                    state = STATES.IDLE;
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
                }
                //Debug.Log("NewVel mag: " + newVel.magnitude);
                //Debug.Log("NewVel angle: " + Vector2.Angle(playerRB.velocity, newVel));

                if (!(Vector2.Angle(playerRB.velocity, newVel) < 90) || newVel.magnitude < 1.0)
                {
                    state = STATES.STOP_B;
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
                }

                if (ctrls.GetSpeed() > 0)
                {
                    state = STATES.MOVE_F;
                }


                break;
            case STATES.OILED:
                
                //setting newvel direction at unit length
                setNewVelRotationDrifting(ref newVel);
                //change player turning
                setRotationDrifting();

                newVel = newVel * playerRB.velocity.magnitude * 0.99f;

                if (playerRB.velocity.magnitude < speedList[(int)BOOSTS.STANDARD] / 3)
                {
                    state = STATES.MOVE_F;
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

                if ((!ctrls.GetA() && driftTime > minDriftTime) || driftTime > maxDriftTime)
                {
                    StartBoost(BOOSTS.DRIFT, driftTime * Time.fixedDeltaTime);
                    driftTime = 0;
                    
                }
                else if (!ctrls.GetA() && driftTime <= minDriftTime)
                {
                    driftTime = 0;
                    state = STATES.MOVE_F;
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
                
                newVel = newVel * playerRB.velocity.magnitude * 0.9f;

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

        if (state != STATES.COUNTDOWN || !finished)
        {
            DoPowerups();
        }

        animator.SetFloat("TurnVal", -ctrls.GetTurn());
        charAnimator.SetFloat("TurnVal", -ctrls.GetTurn());
        float animspeed = newVel.magnitude;
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
        StartCoroutine(endGhosted(1f));
        state = STATES.IDLE;
    }

    IEnumerator endGhosted(float time)
    {
        yield return new WaitForSeconds(time);
        setGhosted(false);
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
}
