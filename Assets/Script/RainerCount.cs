using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainerCount : MonoBehaviour {

    private Text text;
    private int value;

    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            text.text = $"RainerCount {value}";
        }
    }

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
