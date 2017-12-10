using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public static class Barnout {

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public static void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    //Returns true when a button is held
    public static bool ButtonHeld(ButtonState state)
    {
        return state == ButtonState.Pressed;
    }

    //Returns true on the first frame a button is pressed
    public static bool ButtonPressed(ButtonState state, ButtonState prevState)
    {
        return state == ButtonState.Pressed && prevState == ButtonState.Released;
    }
}
