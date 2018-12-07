using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGauge : MonoBehaviour {
    CircularGauge circularGauge;

    private float[] value = new float[4];
    private float t;

	// Use this for initialization
	void Start () {
        circularGauge = GetComponent<CircularGauge>();
        t = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(t < Mathf.PI * 0.5f)
        {
            t += 0.02f;
            value[1] = Mathf.Cos(t);
            value[0] = 1 - value[1];
            circularGauge.Values = value;
        }
        else
        {
            value[0] = 1;
            value[1] = 0;
            circularGauge.Values = value;

            enabled = false;
        }
	}
}
