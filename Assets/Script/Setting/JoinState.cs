using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinState : MonoBehaviour {

    public PlayerIcon playerIcon;
    private Text joinState;

	// Use this for initialization
	void Start () {

        joinState = GetComponent<Text>();
		
	}
	
	// Update is called once per frame
	void Update () {

        if(playerIcon.IsJoin)
        {
            joinState.text = ("JOINING");
        }
        else
        {
            joinState.text = ("NOT JOIN");
        }
	}
}
