using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePainter : MonoBehaviour
{
    public new Camera camera;

    [Range(0,3)]
    public int playerNo = 0;

    private int mask;

    private void Start()
    {
        if(camera == null)
            camera = GameObject.Find("CameraDebug").GetComponent<Camera>();

        mask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        if (!Input.GetMouseButton(1))
            return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
        {
            var paintObject = hitInfo.transform.parent.GetComponent<Ground>();
            if (paintObject != null)
            {
                paintObject.GrowGrass(hitInfo.textureCoord, playerNo);
            }
        }
        
    }
}
