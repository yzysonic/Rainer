using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RainerLib;

[RequireComponent(typeof(RawImage))]
public class FadeInOut : Singleton<FadeInOut> {

    public const float fadeTime = 1.0f;

    private RawImage image;
    private bool FI,FO;
    private System.Action callBackIn, callBackOut;
    private Timer timer;

    public float Alpha
    {
        get
        {
            return image.color.a;
        }
        set
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, value);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        var canvas = GameObject.Find("Canvas");
        if(canvas == null)
        {
            var com = gameObject.AddComponent<Canvas>();
            com.renderMode = RenderMode.ScreenSpaceOverlay;
            com.sortingOrder = 100;
        }
        else
        {
            transform.SetParent(canvas.transform);
        }


        image = GetComponent<RawImage>();
        image.color = Color.black;

        var rectTransform = image.rectTransform;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;

        FI = false;
        FO = false;

        timer = new Timer(fadeTime);

        enabled = false;
    }

    private void FixedUpdate()
    {
        timer++;

        if(FI == true && FO == false)
        {
            Alpha = Mathf.Max(0.0f, 1.0f - timer.Progress);

            if (Alpha <= 0f)
            {
                FI = false;
                enabled = false;

                callBackIn?.Invoke();
            }

        }
        else if(FI == false && FO == true)
        {
            Alpha = Mathf.Min(1.0f, timer.Progress);

            if (Alpha >= 1f)
            {
                FO = false;
                enabled = false;

                callBackOut?.Invoke();
            }

        }
    }

    public void FadeIn(float fadeTime, System.Action callBack = null)
    {
        FI = true;
        this.callBackIn = callBack;
        timer.Reset(fadeTime);
        enabled = true;
    }

    public void FadeIn(System.Action callBack = null)
    {
        FadeIn(fadeTime, callBack);
    }

    public void FadeOut(float fadeTime, System.Action callBack = null)
    {
        FO = true;
        this.callBackOut = callBack;
        timer.Reset(fadeTime);
        enabled = true;
    }

    public void FadeOut(System.Action callBack = null)
    {
        FadeOut(fadeTime, callBack);
    }


}
