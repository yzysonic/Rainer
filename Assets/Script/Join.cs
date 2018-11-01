using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Join : MonoBehaviour {

    public JoinState joinState;
    public Text join;
    public bool isJoin;
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

        isJoin = joinState.isJoin;

        if(isJoin == true)
        {
            join.text = ("");
        }
        else
        {
            join.text = ("OButton Join");
        }

    }
}
