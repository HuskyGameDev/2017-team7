using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenObject : MonoBehaviour
{
    public float rotation = 0;
    public Player owner;
    int collisions = 0;

    public EggSplat[] eggSplats;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(clearOwner(2));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerWallCollider"  && collision.gameObject.GetComponentInParent<Player>() != owner)
        {

            if (collision.gameObject.GetComponentInParent<Player>().state != Player.STATES.FLYING)
            {
                collisions++;
                eggSplats[collision.gameObject.GetComponentInParent<Player>().playerNumber - 1].enableSplat(2f);
                if (collisions >= 1)
                    Destroy(gameObject);
            }

        }
    }

    IEnumerator clearOwner(float time)
    {
        yield return new WaitForSeconds(time);
        owner = null;
    }
}
