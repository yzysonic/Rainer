using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cancel : MonoBehaviour {

    public JoinState joinState;
    public Text cancel;
    public bool isJoin;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        isJoin = joinState.isJoin;

        if(isJoin == true)
        {
            cancel.text = ("XButton Cancel");
        }
        else
        {
            cancel.text = ("");
        }
	}
}
