using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFallow : MonoBehaviour {

    public new Camera camera;
    public Vector3 offsetWorld;
    public Vector2 offsetLocal;

    [SerializeField]
    private Transform target;
    private RectTransform rectTransform;
    private Vector2 sizeDelta;

    public Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
            gameObject.SetActive(target != null);
        }
    }

	void Start ()
    {
        rectTransform = GetComponent<RectTransform>();
        sizeDelta = transform.parent.GetComponent<RectTransform>().sizeDelta;
        for (var parent = transform.parent; parent != null; parent = parent.parent)
        {
            var canvas = parent.GetComponent<Canvas>();            
            if (canvas != null)
            {
                sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
                break;
            }
        }
	}


    private void LateUpdate()
    {
        rectTransform.localPosition = (camera.WorldToViewportPoint(target.position + offsetWorld) - 0.5f * Vector3.one) * sizeDelta + offsetLocal;
    }
}
