using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class settingTotal : MonoBehaviour {

    public string nextScene = "GameScene";
    public PlayerIcon[] playerIcon;
    public bool[] isJoin;
    public int[] numPlayer;
    public int countPlayer;
    public bool canStart;
    private List<Joycon> joycons;

    // Use this for initialization
    void Start () {

        playerIcon = new PlayerIcon[4];
        isJoin = new bool[4];
        numPlayer = new int[4];
        countPlayer = 0;
        joycons = JoyconManager.Instance.j;

        playerIcon[0] = GameObject.Find("1P").GetComponent<PlayerIcon>();
        playerIcon[1] = GameObject.Find("2P").GetComponent<PlayerIcon>();
        playerIcon[2] = GameObject.Find("3P").GetComponent<PlayerIcon>();
        playerIcon[3] = GameObject.Find("4P").GetComponent<PlayerIcon>();

        FadeInOut.Instance.FadeIn();

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

        foreach(var joycon in joycons)
        {
            if (!joycon.GetButton(Joycon.Button.DPAD_DOWN))
                continue;
            
            var playerNo = joycon.GetPlayerNo();
            if (playerNo == -1)
                SetJoin(joycon);
            else
                playerIcon[playerNo].setJoin();
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

        if(canStart && Input.GetKeyDown(KeyCode.Return) && !FadeInOut.Instance.enabled)
        {
            GameSetting.PlayerCount = countPlayer;
            FadeInOut.Instance.FadeOut(() => SceneManager.LoadScene(nextScene));
        }
    }

    void SetJoin(Joycon joycon)
    {
        for(var i = 0; i < 4; i++)
        {
            if(playerIcon[i].numJoin == 0)
            {
                playerIcon[i].setJoin();
                joycon.BindPlayer(i);
            }
        }
    }
}
