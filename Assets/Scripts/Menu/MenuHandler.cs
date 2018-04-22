using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

    public GameObject MainMenu;
    public GameObject CharSelect;

    private GameObject _MainMenu;
    private GameObject _CharSelect;

    public MM_AudioMaster audioMaster;

	// Use this for initialization
	void Start ()
    {
        audioMaster.PlayMusic();
        _MainMenu = Instantiate(MainMenu.gameObject);
	}
	
	public void ToCharSelect()
    {
        audioMaster.ChangeToDrums();
        Destroy(_MainMenu.gameObject);
        _CharSelect = Instantiate(CharSelect.gameObject);
    }

    public void ToMainMenu()
    {
        audioMaster.ChangeToNormal();
        Destroy(_CharSelect.gameObject);
        _MainMenu = Instantiate(MainMenu.gameObject);
    }
}
