using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartClick : MonoBehaviour {
    public void onClickStart(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
