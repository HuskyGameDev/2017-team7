using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D playerRB;
    public float acceleration = 0;
	public float turningSpeed = 0.5f;
	public float maxSpeed = 5.0f;
	public string playerNumber;

    // Use this for initialization
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("LeftStickX-" + playerNumber);
        float moveVertical = Input.GetAxis("RightTrigger-" + playerNumber);


		//Add torque to turn
		playerRB.AddTorque (moveHorizontal*turningSpeed);
		Vector2 newVel = new Vector2();
		Vector2 accel = new Vector2 ();
		// We get the rotation, convert to radians, and also add 90 degrees (PI/2 radians) to get our direction angle.
		float rotation = (float)(Math.PI/180.0) * playerRB.rotation + (float)(Math.PI/2.0);
		//new vel should be in the direction of rotation
		newVel.Set ((float)Math.Cos (rotation), (float)Math.Sin (rotation));

		accel = newVel * acceleration * moveVertical;
		//Clamp to max speed
		newVel = Vector2.ClampMagnitude((newVel * playerRB.velocity.magnitude) + accel, maxSpeed);

		playerRB.velocity = newVel;
	}

    void FixedUpdate()
    {

    }
}
