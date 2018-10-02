using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[ExecuteInEditMode]
public class CameraFallow : MonoBehaviour {

    public Transform target;

    [Range(3, 40)]
    public float distance = 20;

    [Range(10, 80)]
    public float angle = 45;
    private Vector3 offset;

    void OnValidate()
    {
        var pos = new Vector3();
        var phi = target.transform.rotation.eulerAngles.y;
        var theta = angle * Mathf.Deg2Rad;

        pos.x = distance * Mathf.Cos(theta) * Mathf.Sin(phi);
        pos.z = -distance * Mathf.Cos(theta) * Mathf.Cos(phi);
        pos.y = distance * Mathf.Sin(theta);

        offset = pos - target.position;
    }

    // Use this for initialization
    void Start () {
        var pos = new Vector3();
        var phi = target.transform.rotation.eulerAngles.y;
        var theta = angle * Mathf.Deg2Rad;

        pos.x = distance * Mathf.Cos(theta) * Mathf.Sin(phi);
        pos.z = -distance * Mathf.Cos(theta) * Mathf.Cos(phi);
        pos.y = distance * Mathf.Sin(angle);

        offset = pos - target.position;
	}

    private void Update()
    {
        transform.position = target.position + target.transform.rotation * offset;
        transform.LookAt(target);
    }

    // Update is called once per frame
    void FixedUpdate () {
        //transform.position = target.position + target.transform.rotation * offset;
        //transform.LookAt(target);
    }
}
