using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Credits : MenuButton {
    public override void ActivateButton()
    {
        Barnout.ChangeScene("CharacterSelectScene");
    }

}
