using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Transform))]
public class CameraFallow : MonoBehaviour {

    public Transform target;

    private Vector3 pos_offset;
    private Quaternion init_rotation;

	// Use this for initialization
	void Start () {
        pos_offset = transform.position - target.transform.position;
        init_rotation = transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position + target.rotation * pos_offset;
        transform.rotation = target.rotation * init_rotation;
    }
}
