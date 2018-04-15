using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionStickPowerup : MonoBehaviour {

    [UnityEngine.SerializeField]
    public SpriteRenderer coin;

    public void SetTaken(Sprite newCoin)
    {
        coin.gameObject.SetActive(true);
        coin.sprite = newCoin;
    }

    // Use this for initialization
    void Start () {
        coin.gameObject.SetActive(false);
	}
}
