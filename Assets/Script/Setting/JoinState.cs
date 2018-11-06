using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinState : MonoBehaviour {

    public PlayerIcon playerIcon;
    public bool isJoin;
    public Text IsJoin;

	// Use this for initialization
	void Start () {


        //isJoin = playerIcon.isJoin;

		
	}
	
	// Update is called once per frame
	void Update () {

        if(isJoin == false)
        {
            IsJoin.text = ("NOT JOIN");
        }
        else
        {
            IsJoin.text = ("JOINING");
        }
	}
}
