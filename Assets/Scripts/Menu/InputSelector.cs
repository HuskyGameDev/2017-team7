using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputSelector : MonoBehaviour
{

    public EventSystem eventSystem;
    public GameObject selectedObject; 

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //selectedObject = eventSystem.firstSelectedGameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(eventSystem.currentSelectedGameObject);
        if (eventSystem.currentSelectedGameObject != selectedObject)
        {
            if (eventSystem.currentSelectedGameObject == null)
                eventSystem.SetSelectedGameObject(selectedObject);
            else
                selectedObject = eventSystem.currentSelectedGameObject;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

}
