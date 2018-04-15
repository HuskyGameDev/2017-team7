using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCoinStanding : MonoBehaviour {

    [UnityEngine.SerializeField]
    private GameObject _playerCoin;
    [UnityEngine.SerializeField]
    private GameObject _charCoin;


    public void SetCoins(Sprite playerCoin, Sprite charCoin)
    {
        _playerCoin.GetComponent<SpriteRenderer>().sprite = playerCoin;
        _charCoin.GetComponent<SpriteRenderer>().sprite= charCoin;
    }


}
