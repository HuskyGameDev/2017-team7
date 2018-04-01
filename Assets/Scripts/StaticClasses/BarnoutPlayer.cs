using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnoutPlayer {
    private BarnoutPowerup[] _powerups;
    private bool _isActive;
    private int _character;
    private int _playerNum;
    public BarnoutPlayer(bool isActive, int character, int playerNum)
    {
        _isActive = isActive;
        _character = character;
        _playerNum = playerNum;
        _powerups = new BarnoutPowerup[4];
    }

    public bool IsActive() { return _isActive; }

    public int GetPlayerNum() { return _playerNum; }

    public int GetCharacter() { return _character; }

    public void SetPowerup(int index, BarnoutPowerup powerup) { _powerups[index] = powerup; }

    public BarnoutPowerup GetPowerup(int index) { return _powerups[index]; }

}
