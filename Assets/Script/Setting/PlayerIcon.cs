using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour {


    private JoinState joinState;
    public bool isJoin;
    public int numJoin;
	// Use this for initialization
	void Start () {

        joinState = GetComponentInChildren<JoinState>();
        isJoin = false;
        numJoin = 0;

	}
	
	// Update is called once per frame
	void Update () {

        joinState.isJoin = isJoin;
    }

    public void setJoin()
    {
        isJoin = !isJoin;

        if (isJoin == true)
            numJoin = 1;
        else if (isJoin == false)
            numJoin = 0;
    }
}
