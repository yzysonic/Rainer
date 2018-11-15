using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFallow : MonoBehaviour
{

    public Transform target;
    public Transform lookat;

    [SerializeField, Range(0.0f, 0.5f * Mathf.PI)]
    private float angle = 0.397f;

    [SerializeField]
    private float distance = 10.0f;

    void Start()
    {
        if (target == null && lookat == null)
            enabled = false;
    }

    void LateUpdate()
    {
        UpdatePosition();
        transform.LookAt(lookat ?? target);
    }

    public void UpdatePosition()
    {
        if (target == null)
            return;
        
        var pos = new Vector3();
        var rot_y = target.eulerAngles.y * Mathf.Deg2Rad;

        pos.y = distance * Mathf.Sin(angle);
        pos.x = -distance * Mathf.Cos(angle) * Mathf.Sin(rot_y);
        pos.z = -distance * Mathf.Cos(angle) * Mathf.Cos(rot_y);
        pos += target.position;

        transform.position = pos;

    }
}
