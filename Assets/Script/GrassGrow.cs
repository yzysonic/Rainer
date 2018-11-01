using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGrow : MonoBehaviour {

    private float y;
    //public GameObject obj;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, -0.2f, transform.position.z);
        //y = -0.2f;
        //obj = new GameObject();
        //obj = transform.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.Translate(0.0f, 0.1f, 0.0f);

        if (transform.position.y > 0.4f)
        {
            enabled = false;
        }

    }
}
