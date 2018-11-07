using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

    public settingTotal total;
    public AnimationCurve Scl;
    private float myScl;
    public Button myButton;
    public float Red,Green,Bule;


	// Use this for initialization
	void Start () {

        total = GetComponentInParent<settingTotal>();
        myButton = GetComponent<Button>();

    }
	
	// Update is called once per frame
	void Update () {

        if (total.CanStart)
        {
            Red = 1f;
            Green = 0.5f;
            Bule = 0;
        }
        else
        {
            Red = Green = Bule = 0.8f;
        }

        myButton.image.color = new Color(Red,Green,Bule,myButton.image.color.a);
    }

}
