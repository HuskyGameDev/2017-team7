using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Options : MenuButton {

    public MenuHandler menuHandler;

    public override void ActivateButton()
    {
        menuHandler.ToCharSelect();
        //Barnout.ChangeScene("CharacterSelectScene");
    }
}
