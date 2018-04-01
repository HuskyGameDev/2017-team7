using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnoutPowerup {
    private PowerupType _powerup;
    private uint _amount;
    public BarnoutPowerup(PowerupType powerup, uint amount)
    {
        _powerup = powerup;
        _amount = amount;
    }
}
