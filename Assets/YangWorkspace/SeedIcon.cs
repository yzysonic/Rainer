using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class SeedIcon : MonoBehaviour {

    public float interval = 0.7f;
    public float distance = 1.0f;
    public float speed = 10.0f;

    private Vector3 pos;
    private float height;
    private Timer timer;

	// Use this for initialization
	void Start () {
        pos = transform.localPosition;
        height = pos.y;
        timer = new Timer(interval);
	}
	
	// Update is called once per frame
	void Update () {
        timer++;

        if(timer.TimesUp())
        {
            pos.y = height;
            timer.Reset(interval);
        }

        pos.y = Mathf.Lerp(pos.y, height + distance, speed * Time.deltaTime);
        transform.localPosition = pos;
	}
}
