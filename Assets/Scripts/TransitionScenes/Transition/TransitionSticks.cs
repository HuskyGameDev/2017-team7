using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSticks : MonoBehaviour {

    public TransitionStickPowerup powerup3x;
    public TransitionStickPowerup powerup2x_l;
    public TransitionStickPowerup powerup2x_r;
    public TransitionStickPowerup powerup1x;

    public PowerupImageMap powerupImageMap;

    public void Init()
    {
        List<PowerupType> values = new List<PowerupType>((PowerupType[])Enum.GetValues(typeof(PowerupType)));
        SetPowerup(powerup3x, ref values, 3);
        SetPowerup(powerup2x_l, ref values, 2);
        SetPowerup(powerup2x_r, ref values, 2);
        SetPowerup(powerup1x, ref values, 1);
    }
    private void SetPowerup(TransitionStickPowerup powerup, ref List<PowerupType> values, uint amount)
    {
        int rand = UnityEngine.Random.Range(0, values.Count);
        powerup.SetPowerup(new BarnoutPowerup(values[rand], amount), powerupImageMap);
        values.RemoveAt(rand);
    }

    private TransitionStickPowerup GetPowerupStick(PowerupDirection direction)
    {
        switch(direction)
        {
            case PowerupDirection.DOWN: return powerup1x;
            case PowerupDirection.LEFT: return powerup2x_l;
            case PowerupDirection.RIGHT: return powerup2x_r;
            case PowerupDirection.UP: return powerup3x;
        }
        return null;
    }

    public BarnoutPowerup SetTaken(PowerupDirection direction, Sprite coin)
    {
        TransitionStickPowerup p = GetPowerupStick(direction);
        return p.SetTaken(coin);
    }

    public bool IsTaken(PowerupDirection direction)
    {
        TransitionStickPowerup p = GetPowerupStick(direction);
        return p.IsTaken();
    }




}
