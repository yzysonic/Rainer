using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLogo : MonoBehaviour {

    float timer;

    private void OnEnable()
    {
        timer = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        //if(timer)
	}
}
