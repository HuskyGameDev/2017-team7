using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{

    private Rigidbody2D player2;
    public int speed = 0;

    // Use this for initialization
    void Start()
    {
        player2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal2");
        float moveVertical = Input.GetAxis("Vertical2");

        Vector2 movement = new Vector2(moveHorizontal * speed, moveVertical * speed);

        player2.AddForce(movement);
    }
}
