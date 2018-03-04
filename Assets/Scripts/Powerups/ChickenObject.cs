using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenObject : MonoBehaviour
{
    public int collisions = -1;
    public float rotation = 0;

    public EggSplat[] eggSplats;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)) && collision.gameObject.tag == "Player")
        {
            collisions++;
            if (collisions == 1)
            {
                eggSplats[collision.gameObject.GetComponent<Player>().playerNumber - 1].enableSplat(2f);
                Destroy(gameObject);
            }
        }
    }
}
