using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Options : MenuButton {

    public MenuHandler menuHandler;

    private void Start()
    {
        menuHandler = FindObjectOfType<MenuHandler>();
    }

    public override void ActivateButton()
    {
        menuHandler.ToCharSelect();
        //Barnout.ChangeScene("CharacterSelectScene");
    }
}
