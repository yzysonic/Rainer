using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public Transform target;
    public float fallow_speed = 2.0f;
    private float height;

	// Use this for initialization
	void Start () {
        height = transform.position.y;
        transform.position = target.position;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0.0f, height, 0.0f), Time.deltaTime*fallow_speed);
	}
}
