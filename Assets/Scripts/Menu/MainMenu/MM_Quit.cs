using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Quit : MenuButton {
    public override void ActivateButton()
    {
        Barnout.Quit();
    }
}
