using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XInputDotNetPure;

public class InputSelector : MonoBehaviour
{

    public EventSystem eventSystem;
    public GameObject[] buttons;
    private int currButton;

    bool mouseControl;

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    bool axisBuffer = true;

    // Use this for initialization
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        mouseControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                    Debug.Log(i);
                    break;
                }
            }
        }

        
        prevState = state;
        state = GamePad.GetState(playerIndex);
        if (!mouseControl)
        {
            Cursor.lockState = CursorLockMode.None;
            if (eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(buttons[0]);
                currButton = 0;
                axisBuffer = false;
            }
            else
            {
                if (state.ThumbSticks.Left.Y > 0 && axisBuffer)
                {
                    
                    currButton++;
                    if (currButton >= buttons.Length) currButton = 0;
                    eventSystem.SetSelectedGameObject(buttons[currButton]);
                    axisBuffer = false;
                }
                else if (state.ThumbSticks.Left.Y < 0 && axisBuffer)
                {
                    currButton--;
                    if (currButton < 0) currButton = buttons.Length - 1;
                    eventSystem.SetSelectedGameObject(buttons[currButton]);
                    axisBuffer = false;
                }
                if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
                {
                    buttons[currButton].GetComponent<Button>().onClick.Invoke();
                }
                if (Input.GetAxisRaw("Mouse_X") != 0 || Input.GetAxisRaw("Mouse_Y") != 0)
                {
                    Cursor.visible = true;
                    mouseControl = true;
                    eventSystem.SetSelectedGameObject(null);
                    currButton = 0;
                }
            }
            
            if (state.ThumbSticks.Left.Y == 0)
            {
                axisBuffer = true;
            }
            
           
        }
        if (mouseControl)
        {
            
            if (state.ThumbSticks.Left.Y != 0)
            {
                Debug.Log("Adjusted y");
                mouseControl = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                eventSystem.SetSelectedGameObject(buttons[0]);
                axisBuffer = false;
            }
        }
        
    }

}