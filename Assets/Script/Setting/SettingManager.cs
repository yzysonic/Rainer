using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class SettingManager : MonoBehaviour {

    public string nextScene = "GameScene";

    private PlayerIcon[] playerIcon;
    private int countPlayer = 0;
    private int CountPlayer
    {
        get
        {
            return countPlayer;
        }
        set
        {
            CanStart = value >= 2;
            countPlayer = value;
        }
    }
    private bool startButtonDown;
    private List<Joycon> joycons;
    public bool CanStart { get; private set; }

    // Use this for initialization
    void Start () {

        joycons = JoyconManager.Instance.j;

        playerIcon = new PlayerIcon[4];

        playerIcon[0] = GameObject.Find("1P").GetComponent<PlayerIcon>();
        playerIcon[1] = GameObject.Find("2P").GetComponent<PlayerIcon>();
        playerIcon[2] = GameObject.Find("3P").GetComponent<PlayerIcon>();
        playerIcon[3] = GameObject.Find("4P").GetComponent<PlayerIcon>();

        FadeInOut.Instance.FadeIn();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerIcon[0].IsJoin = !playerIcon[0].IsJoin;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerIcon[1].IsJoin = !playerIcon[1].IsJoin;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerIcon[2].IsJoin = !playerIcon[2].IsJoin;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerIcon[3].IsJoin = !playerIcon[3].IsJoin;
        }

        foreach(var joycon in joycons)
        {
            if (joycon.GetButtonDown(GameSetting.Button.Join) && joycon.GetPlayerNo() == -1)
            {
                SetJoin(joycon);
            }
        }

        CountPlayer = playerIcon.Sum(p => p.IsJoin ? 1 : 0);

        startButtonDown = Input.GetKeyDown(KeyCode.Return) || playerIcon.Any(p => p.Joycon?.GetButtonDown(GameSetting.Button.Start) ?? false);

        if (CanStart && startButtonDown && !FadeInOut.Instance.enabled)
        {
            GameSetting.PlayerCount = CountPlayer;
            FadeInOut.Instance.FadeOut(() => SceneManager.LoadScene(nextScene));
        }
    }

    void SetJoin(Joycon joycon)
    {
        for(var i = 0; i < 4; i++)
        {
            if(!playerIcon[i].IsJoin)
            {
                playerIcon[i].Joycon = joycon;
                joycon.BindPlayer(i);
                break;
            }
        }
    }

}
