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
<<<<<<< HEAD
            if (collision.gameObject.GetComponent<Player>().state != Player.STATES.FLYING)
            {
                collisions++;
                eggSplats[collision.gameObject.GetComponent<Player>().playerNumber - 1].enableSplat(2f);
                if (collisions >= 1)
                    Destroy(gameObject);
            }
=======
            collisions++;
            eggSplats[collision.gameObject.GetComponentInParent<Player>().playerNumber - 1].enableSplat(2f);
            if (collisions >= 1)
                Destroy(gameObject);
>>>>>>> 0cc2186ea9c092c02d9200ba8081544b63406c15
        }
    }

    IEnumerator clearOwner(float time)
    {
        yield return new WaitForSeconds(time);
        owner = null;
    }
}
