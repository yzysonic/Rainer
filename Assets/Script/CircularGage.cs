using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularGage : RawImage {

    public float circleWidth = 5.0f;
    public float radius = 10.0f;

    protected override void OnValidate()
    {
       material.SetFloat("_Radius", radius);
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        material.SetFloat("_Radius", radius);
    }
}
