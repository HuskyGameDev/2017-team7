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
        if (PlayerData.playerChars[player.playerNumber - 1] < 0)
        {
            mmIcon.gameObject.SetActive(false);
        }
        else
        {
            mmIcon.sprite = PlayerData.charIcons[PlayerData.playerChars[player.playerNumber - 1]];
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        playerDist = new Vector3(ref1.position.x - player.playerRB.position.x,
                                 ref1.position.y - player.playerRB.position.y, 0);
        iconPos = new Vector3(playerDist.x / 3.8f + ref2.position.x,
                             playerDist.y / 3.8f + ref2.position.y, 0);

        mmIcon.rectTransform.position = iconPos;
    }
}
