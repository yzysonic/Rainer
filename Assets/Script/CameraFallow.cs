using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour {

    public Transform target;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - target.position;
	}
	
	// Update is called once per frame
    void FixedUpdate () {
        transform.position = target.position + target.transform.rotation * offset;
        transform.LookAt(target);
	}
}
