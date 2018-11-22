using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyconImage : MonoBehaviour {

    private PlayerIcon playerIcon;
    private RawImage image;
    private Color color;

    // Use this for initialization
    void Start()
    {
        playerIcon = GetComponentInParent<PlayerIcon>();
        image = GetComponent<RawImage>();
        color = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIcon.IsJoin)
        {
            color.a = 0.0f;
        }
        else
        {
            color.a = 0.7f;
        }
        
        image.color = color;
    }
}
