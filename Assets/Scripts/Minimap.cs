using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

    public RectTransform location;
    private Vector2 iconPos;
    public Player player;

	// Use this for initialization
	void Start () {
        iconPos = new Vector2(player.playerRB.position.x + 795, player.playerRB.position.y - 411);
        location.position.Set(0, 0, 0);
        Debug.Log(iconPos.ToString());

    }
	
	// Update is called once per frame
	void Update () {
        location.position.Set(0, 0, 0);
        Debug.Log(location.position.ToString());
    }
}
