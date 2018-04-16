using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionButton : MonoBehaviour {

    public SpriteRenderer powerup;
    private bool taken;

    private void Awake()
    {
        ResetPowerup();
    }

    public void ResetPowerup()
    {
        powerup.gameObject.SetActive(false);
    }

    public bool IsTaken()
    {
        return taken;
    }

    public void SetTaken(PowerupType powerupImage, PowerupImageMap powerupImageMap)
    {
        powerup.gameObject.SetActive(true);
        powerup.sprite = powerupImageMap.GetImage(powerupImage);
        //TODO create way to get images for powerups
        //powerup.sprite = powerupImage;
    }
}
