using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionStickPowerup : MonoBehaviour {

    [UnityEngine.SerializeField]
    private SpriteRenderer coin;
    [UnityEngine.SerializeField]
    private SpriteRenderer powerupImage;

    bool taken = false;
    private BarnoutPowerup _powerup;

    public BarnoutPowerup SetTaken(Sprite newCoin)
    {
        coin.gameObject.SetActive(true);
        coin.sprite = newCoin;
        taken = true;
        return _powerup;
    }

    public bool IsTaken() { return taken; }

    public void SetPowerup(BarnoutPowerup powerup, PowerupImageMap imageMap)
    {
        _powerup = powerup;
        powerupImage.sprite = imageMap.GetImage(powerup.GetPowerup());
    }

    // Use this for initialization
    void Start () {
        coin.gameObject.SetActive(false);
        powerupImage = GetComponent<SpriteRenderer>();
    }
}
