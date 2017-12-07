using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    public Player player;
    public Image mmIcon;
    public Transform ref1, ref2;
    Vector3 playerDist, iconPos;

	// Use this for initialization
	void Start () {
       // mmIcon.rectTransform.position = new Vector3(0, 0, 0);
        Debug.Log(mmIcon.rectTransform.position.ToString());

    }
	
	// Update is called once per frame
	void Update () {
        playerDist = new Vector3(ref1.position.x - player.playerRB.position.x,
                                 ref1.position.y - player.playerRB.position.y, 0);
        iconPos = new Vector3(playerDist.x / 4 + ref2.position.x,
                             playerDist.y / 4+ ref2.position.y, 0);

        mmIcon.rectTransform.position = iconPos;
    }
}
