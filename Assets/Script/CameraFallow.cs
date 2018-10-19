using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFallow : MonoBehaviour
{

    public Transform target;
    public Transform lookat;

    static public float Angle { get; set; } = 0.397f;
    static public float Distance { get; set; } = 10.0f;

    void Start()
    {
        if (target == null)
            enabled = false;
    }

    void Update()
    {
        UpdataPosition();
    }

    public void UpdataPosition()
    {
        var pos = new Vector3();
        var rot_y = target.eulerAngles.y * Mathf.Deg2Rad;

        pos.y =  Distance * Mathf.Sin(Angle);
        pos.x = -Distance * Mathf.Cos(Angle) * Mathf.Sin(rot_y);
        pos.z = -Distance * Mathf.Cos(Angle) * Mathf.Cos(rot_y);
        pos += target.position;

        transform.position = pos;
        transform.LookAt(lookat ? lookat : target);
    }
}
