using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

    public RawImage image;
    public float Alpha;
    public bool FI,FO;

    System.Action callBackIn,callBackOut;

    private void Start()
    {

        image = GetComponent<RawImage>();
        Alpha = image.color.a;
        FI = false;
        FO = false;




    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            fadeIn();

        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            fadeOut();

        }

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

        image.color = new Color(image.color.r, image.color.g, image.color.b, Alpha);
        Alpha = image.color.a;


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
