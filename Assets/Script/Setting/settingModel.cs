using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingModel : MonoBehaviour {

    public RawImage model;
    public JoinState joinState;
    public float Alpha;
    public bool isJoin;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        isJoin = joinState.isJoin;

        if (isJoin)
        {
            Alpha = 255;
        }
        else
        {
            Alpha = 0;
        }

        model.color = new Color(model.color.r, model.color.g, model.color.b, Alpha);
	}
}
