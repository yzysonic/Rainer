using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class SettingManager : MonoBehaviour {

    public string nextScene = "GameScene";
    public GameObject PlayerField;
    public Light spotLight;

    private PlayerIcon[] playerIcons;
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
    private float intensity;

    private void Awake()
    {
        joycons = JoyconManager.Instance.j;

        playerIcons = new PlayerIcon[4];

        playerIcons[0] = GameObject.Find("1P").GetComponent<PlayerIcon>();
        playerIcons[1] = GameObject.Find("2P").GetComponent<PlayerIcon>();
        playerIcons[2] = GameObject.Find("3P").GetComponent<PlayerIcon>();
        playerIcons[3] = GameObject.Find("4P").GetComponent<PlayerIcon>();
        intensity = spotLight.intensity;

        foreach (var joycon in joycons)
        {
            joycon.UnbindPlayer();
        }

        GameSetting.PlayerColors = new Color[GameSetting.DefaultPlayerColors.Length];
        System.Array.Copy(GameSetting.DefaultPlayerColors, GameSetting.PlayerColors, GameSetting.DefaultPlayerColors.Length);

    }

    // Use this for initialization
    void Start () {

        FadeInOut.Instance.FadeIn();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerIcons[0].IsJoin = !playerIcons[0].IsJoin;
            playerIcons[0].ColorIndex = playerIcons[0].IsJoin ? 0 : -1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerIcons[1].IsJoin = !playerIcons[1].IsJoin;
            playerIcons[1].ColorIndex = playerIcons[1].IsJoin ? 1 : -1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerIcons[2].IsJoin = !playerIcons[2].IsJoin;
            playerIcons[2].ColorIndex = playerIcons[2].IsJoin ? 2 : -1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerIcons[3].IsJoin = !playerIcons[3].IsJoin;
            playerIcons[3].ColorIndex = playerIcons[3].IsJoin ? 3 : -1;
        }

        foreach(var joycon in joycons)
        {
            if (!joycon.GetButtonDown(GameSetting.JoyconButton.Join) || joycon.GetPlayerNo() >= 0)
            {
                continue;
            }

            for(var i=0;i<playerIcons.Length;i++)
            {
                if (!playerIcons[i].IsJoin)
                {
                    playerIcons[i].Joycon = joycon;
                    joycon.BindPlayer(i);
                    break;
                }
            }
        }

        CountPlayer = playerIcons.Sum(p => p.IsJoin ? 1 : 0);

        bool startButtonDown = Input.GetKeyDown(KeyCode.Return) || playerIcons.Any(p => p.Joycon?.GetButtonDown(GameSetting.JoyconButton.Start) ?? false);

        if (CanStart && startButtonDown && !FadeInOut.Instance.enabled)
        {
            GameSetting.PlayerCount = CountPlayer;

            var activePlayerIcon = playerIcons.Where(pi => pi.IsJoin);
            GameSetting.PlayerColors = activePlayerIcon.Select(pi=>pi.Color).ToArray();
            GameSetting.PlayerColorIndex = activePlayerIcon.Select(pi => pi.ColorIndex).ToArray();

            BGMPlayer.Instance.AudioFades[0].Out();
            BGMPlayer.Instance.AudioFades[1].Out();
            FadeInOut.Instance.FadeOut(() => {
                BGMPlayer.Instance.Destroy();
                SceneManager.LoadScene(nextScene);
            });
        }

        var rot = 2.0f*playerIcons.Sum(p => -p.Joycon?.GetStick()[0] ?? 0.0f);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            rot += Input.GetAxis("KeyMoveX");
        }

        if (Mathf.Abs(rot) > 0.1f)
        {
            PlayerField.transform.Rotate(0, rot * 60.0f * Time.deltaTime, 0);
            spotLight.intensity = Mathf.Lerp(spotLight.intensity, 0.0f, 5.0f * Time.deltaTime);
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
            spotLight.intensity = Mathf.Lerp(spotLight.intensity, intensity, 5.0f * Time.deltaTime);
        }
    }
    
}
