using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Es.TexturePaint.Sample
{
    public class MousePainter : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                var ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    var paintObject = hitInfo.transform.GetComponent<Ground>();
                    if (paintObject != null)
                    {
                        paintObject.PaintGrass(hitInfo.textureCoord);
                    }
                }
            }
        }
    }
}