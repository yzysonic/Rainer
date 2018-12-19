using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class StartGauge : MonoBehaviour {
    CircularGauge circularGauge;

    private float[] value = new float[4];
    private Timer timer;

	// Use this for initialization
	void Start () {
        circularGauge = GetComponent<CircularGauge>();
        timer = new Timer(2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(!timer.TimesUp())
        {
            timer++;
            value[0] = 1 - Mathf.Cos(timer.Progress * Mathf.PI * 0.5f);
            value[1] = 1 - Mathf.Sin(timer.Progress * Mathf.PI * 0.5f);
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
