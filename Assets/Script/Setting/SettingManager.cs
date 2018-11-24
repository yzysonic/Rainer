using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class SettingManager : MonoBehaviour {

    public string nextScene = "GameScene";
    public GameObject PlayerField;

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

        for(var i =0; i<joycons.Count; i++)
        {
            if (joycons[i].GetButtonDown(GameSetting.Button.Join) && joycons[i].GetPlayerNo() == -1)
            {
                playerIcon[i].Joycon = joycons[i];
                joycons[i].BindPlayer(i);
            }
        }

        CountPlayer = playerIcon.Sum(p => p.IsJoin ? 1 : 0);

        bool startButtonDown = Input.GetKeyDown(KeyCode.Return) || playerIcon.Any(p => p.Joycon?.GetButtonDown(GameSetting.Button.Start) ?? false);

        if (CanStart && startButtonDown && !FadeInOut.Instance.enabled)
        {
            GameSetting.PlayerCount = CountPlayer;
            FadeInOut.Instance.FadeOut(() => SceneManager.LoadScene(nextScene));
        }

        //if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Tab)) || playerIcon.Any(p => (p.Joycon?.GetStick()[0] ?? 0.0f) < -0.1f))
        //{
        //    PlayerCamera.transform.position = Quaternion.Euler(0, -1, 0) * PlayerCamera.transform.position;
        //    PlayerCamera.transform.LookAt(new Vector3(0, PlayerCamera.transform.position.y, 0));
        //}
        //else if (Input.GetKey(KeyCode.Tab) || playerIcon.Any(p => (p.Joycon?.GetStick()[0] ?? 0.0f) > 0.1f))
        //{
        //    PlayerCamera.transform.position = Quaternion.Euler(0, 1, 0) * PlayerCamera.transform.position;
        //    PlayerCamera.transform.LookAt(new Vector3(0, PlayerCamera.transform.position.y, 0));
        //}

        if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Tab)) || playerIcon.Any(p => (p.Joycon?.GetStick()[0] ?? 0.0f) < -0.1f))
        {
            PlayerField.transform.Rotate(0, -1, 0);
        }
        else if (Input.GetKey(KeyCode.Tab) || playerIcon.Any(p => (p.Joycon?.GetStick()[0] ?? 0.0f) > 0.1f))
        {
            PlayerField.transform.Rotate(0, 1, 0);
        }
        else
        {
            var direction = PlayerField.transform.rotation.eulerAngles;
            var deltaRotate = direction.y % 90;
            if (deltaRotate != 0.0f)
            {
                if (deltaRotate < 45)
                {
                    direction = Vector3.Lerp(direction, new Vector3(direction.x, direction.y - deltaRotate, direction.z), 0.1f);
                }
                else
                {
                    direction = Vector3.Lerp(direction, new Vector3(direction.x, direction.y - deltaRotate + 90, direction.z), 0.1f);
                }
                PlayerField.transform.rotation = Quaternion.Euler(direction);
            }
        }
    }
    
}
