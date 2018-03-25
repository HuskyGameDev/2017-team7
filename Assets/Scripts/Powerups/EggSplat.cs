using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSplat : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator endSplat(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    public void enableSplat(float time)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(endSplat(time));
    }
}
