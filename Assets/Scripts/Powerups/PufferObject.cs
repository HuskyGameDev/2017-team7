using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferObject : MonoBehaviour {

    public float rotation = 0;
    public Player owner;
    int collisions = 0;

    Collider2D[] hitColliders;

    bool collided = false;

    // Use this for initialization
    void Start () {
        StartCoroutine(clearOwner(2));        
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collided)
        {
            if (collision.tag == "PlayerWallCollider" && collision.gameObject.GetComponentInParent<Player>() != owner)
            {
                collisions++;

                hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 45f);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    Debug.Log(hitColliders[i]);
                    if (hitColliders[i].tag == "PlayerWallCollider")
                    {
                        if (hitColliders[i].gameObject.GetComponentInParent<Player>().state != Player.STATES.FLYING)
                        {
                            hitColliders[i].gameObject.GetComponentInParent<Player>().StartIncap(2f);
                        }
                    }
                }
                if (collisions >= 1)
                    GetComponent<Animator>().SetTrigger("BlowUp");
            }
        }
        
    }

    IEnumerator clearOwner(float time)
    {
        yield return new WaitForSeconds(time);
        owner = null;
    }

    void CallDestroy()
    {
        Destroy(gameObject);
    }

}
