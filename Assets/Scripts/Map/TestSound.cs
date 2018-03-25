using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour {
   

	// Use this for initialization
	void Start () {
        Debug.Log("Starting test sound");
        StartCoroutine(playAfter3());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator playAfter3()
    {
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("3 seconds");

        AkSoundEngine.PostEvent("Play_Barn", gameObject);
    }

}
