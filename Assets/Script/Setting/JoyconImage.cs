using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyconImage : MonoBehaviour {

    public PlayerIcon playerIcon;
    private RawImage image;
    private Color color;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<RawImage>();
        color = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        float prevAlpha = color.a;

        if (playerIcon.IsJoin)
        {
            color.a = 0.0f;
        }
        else
        {
            color.a = 1.0f;
        }

        if (prevAlpha != color.a)
        {
            image.color = color;
        }
    }
}
