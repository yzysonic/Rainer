using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFallow : MonoBehaviour {

    public new Camera camera;
    public Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (target == null)
        {
            enabled = false;
        }

        transform.position = camera.WorldToScreenPoint(target.position);

	}
}
