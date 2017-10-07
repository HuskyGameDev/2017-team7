using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D playerRB;
    public int acceleration = 0;
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

        Vector2 movement = new Vector2(moveHorizontal * acceleration, moveVertical * acceleration);

        playerRB.AddForce(movement);
    }

    void FixedUpdate()
    {

    }
}
