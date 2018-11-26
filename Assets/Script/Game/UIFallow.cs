using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFallow : MonoBehaviour {

    public new Camera camera;
    public Vector3 offsetWorld;
    public Vector2 offsetLocal;
    public AnimationClip popupAnimation;

    [SerializeField]
    private Transform target;
    private RectTransform rectTransform;
    private Vector2 sizeDelta;
    private new Animation animation;

    public Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            var oldTarget = target;

            target = value;
            gameObject.SetActive(target != null);

            if(target != null && target != oldTarget)
            {
                animation?.Play("popup");
            }
        }
    }

    protected virtual void Awake()
    {
        if (popupAnimation != null)
        {
            if (animation == null)
            {
                animation = gameObject.AddComponent<Animation>();
            }
            animation.AddClip(popupAnimation, "popup");
            animation.playAutomatically = false;
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
