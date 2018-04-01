using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferObject : MonoBehaviour {

    public float rotation = 0;
    public Player owner;
    int collisions = 0;

    Collider2D[] hitColliders;

    // Use this for initialization
    void Start () {
        StartCoroutine(clearOwner(2));        
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType().Equals(typeof(CapsuleCollider2D)) && collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Player>() != owner)
        {
            collisions++;

            hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 30);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].GetType().Equals(typeof(CapsuleCollider2D)))
                {
                    if (hitColliders[i].gameObject.GetComponent<Player>().state != Player.STATES.FLYING)
                    {
                        hitColliders[i].gameObject.GetComponent<Player>().StartIncap(2f);
                    }
                }
            }
            if (collisions >= 1)
                Destroy(gameObject);
        }
    }

    IEnumerator clearOwner(float time)
    {
        yield return new WaitForSeconds(time);
        owner = null;
    }
}
