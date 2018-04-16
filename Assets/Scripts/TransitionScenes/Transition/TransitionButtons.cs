using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionButtons : MonoBehaviour {

    public TransitionButton buttonDown;
    public TransitionButton buttonLeft;
    public TransitionButton buttonRight;
    public TransitionButton buttonUp;

    public PowerupImageMap powerupImageMap;

    public void InitPowerups(BarnoutPlayer player)
    {
        InitPowerup(player, PowerupDirection.DOWN);
        InitPowerup(player, PowerupDirection.UP);
        InitPowerup(player, PowerupDirection.LEFT);
        InitPowerup(player, PowerupDirection.RIGHT);
    }

    public void InitPowerup(BarnoutPlayer player, PowerupDirection direction)
    {
        BarnoutPowerup p = player.GetPowerup((int)direction);
        TransitionButton b = GetButtonDirection(direction);
        if (p != null) { b.SetTaken(p.GetPowerup(), powerupImageMap); }
        else { b.ResetPowerup(); }
    }

    private TransitionButton GetButtonDirection(PowerupDirection direction)
    {
        switch (direction)
        {
            case PowerupDirection.DOWN: return buttonDown;
            case PowerupDirection.LEFT: return buttonLeft;
            case PowerupDirection.RIGHT: return buttonRight;
            case PowerupDirection.UP: return buttonUp;
        }
        return null;
    }

    

    public void SetTaken(PowerupDirection direction, PowerupType powerup)
    {
        TransitionButton b = GetButtonDirection(direction);
        b.SetTaken(powerup, powerupImageMap);
    }
}
