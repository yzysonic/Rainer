using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingTotal : MonoBehaviour {

    public PlayerIcon[] playerIcon;
    public bool[] isJoin;
    public int[] numPlayer;
    public int countPlayer;
    public bool canStart;

    // Use this for initialization
    void Start () {

        playerIcon = new PlayerIcon[4];
        isJoin = new bool[4];
        numPlayer = new int[4];
        countPlayer = 0;

        playerIcon[0] = GameObject.Find("1P").GetComponent<PlayerIcon>();
        playerIcon[1] = GameObject.Find("2P").GetComponent<PlayerIcon>();
        playerIcon[2] = GameObject.Find("3P").GetComponent<PlayerIcon>();
        playerIcon[3] = GameObject.Find("4P").GetComponent<PlayerIcon>();

    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 4; i++)
        {
            isJoin[i] = playerIcon[i].isJoin;
            numPlayer[i] = playerIcon[i].numJoin;

        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerIcon[0].setJoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerIcon[1].setJoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerIcon[2].setJoin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerIcon[3].setJoin();

        }

        countPlayer = numPlayer[0] + numPlayer[1] + numPlayer[2] + numPlayer[3];

        if(countPlayer < 2)
        {
            canStart = false;
        }
        else
        {
            canStart = true;
        }

    }
}
