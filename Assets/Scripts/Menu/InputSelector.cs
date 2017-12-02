using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputSelector : MonoBehaviour
{

    public EventSystem eventSystem;
    public GameObject firstObject;
    private GameObject selectedObject;

    bool mouseControl;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouseControl)
        {
            Cursor.lockState = CursorLockMode.None;
            if (eventSystem.currentSelectedGameObject != selectedObject)
            {
                if (eventSystem.currentSelectedGameObject == null)
                    eventSystem.SetSelectedGameObject(selectedObject);
                else
                    selectedObject = eventSystem.currentSelectedGameObject;
            }
            if (Input.GetKeyDown("joystick 1 button 0"))
            {
                selectedObject.GetComponent<Button>().onClick.Invoke();
            }
            if (Input.GetAxisRaw("Mouse_X") != 0 || Input.GetAxisRaw("Mouse_Y") != 0)
            {
                Cursor.visible = true;
                mouseControl = true;
                eventSystem.SetSelectedGameObject(null);
            }
        }
        if (mouseControl)
        {
            
            if (Input.GetAxis("LeftStickY-1") != 0)
            {
                mouseControl = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                eventSystem.SetSelectedGameObject(firstObject);
            }
        }
        
    }

}
