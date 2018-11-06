using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class FadeInOut : MonoBehaviour {


    public RawImage image;
    private bool FI,FO;
    private System.Action callBackIn, callBackOut;
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

    private void Awake()
    {
        FI = false;
        FO = false;
    }

    private void Update()
    {

        if (image.color.a <= 0f)
        {
            FI = false;

            if(callBackIn != null)
            {
                callBackIn();
            }
        }
        if (image.color.a >= 1f)
        {
            FO = false;

            if(callBackOut != null)
            {
                callBackOut();
            }
        }

    }

    private void FixedUpdate()
    {


        if(FI == true && FO == false)
        {
            Alpha -= 0.01f;
        }
        else if(FI == false && FO == true)
        {
            Alpha += 0.01f;
        }

    }

    public void fadeIn(System.Action callBack = null)
    {
        FI = true;
        this.callBackIn = callBack;

    }

    public void fadeOut(System.Action callBack = null)
    {
        FO = true;

        this.callBackOut = callBack;


    }


}
