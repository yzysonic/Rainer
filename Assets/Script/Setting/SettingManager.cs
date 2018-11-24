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
    private float intensity;

    // Use this for initialization
    void Start () {

        joycons = JoyconManager.Instance.j;

        playerIcon = new PlayerIcon[4];

        playerIcon[0] = GameObject.Find("1P").GetComponent<PlayerIcon>();
        playerIcon[1] = GameObject.Find("2P").GetComponent<PlayerIcon>();
        playerIcon[2] = GameObject.Find("3P").GetComponent<PlayerIcon>();
        playerIcon[3] = GameObject.Find("4P").GetComponent<PlayerIcon>();
        intensity = spotLight.intensity;

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
            if (joycons[i].GetButtonDown(GameSetting.JoyconButton.Join))
            {
                playerIcon[joycons[i].GetPlayerNo()].Joycon = joycons[i];
            }
        }

        CountPlayer = playerIcon.Sum(p => p.IsJoin ? 1 : 0);

        bool startButtonDown = Input.GetKeyDown(KeyCode.Return) || playerIcon.Any(p => p.Joycon?.GetButtonDown(GameSetting.JoyconButton.Start) ?? false);

        if (CanStart && startButtonDown && !FadeInOut.Instance.enabled)
        {
            GameSetting.PlayerCount = CountPlayer;
            BGMPlayer.Instance.Fade.Out();
            FadeInOut.Instance.FadeOut(() => {
                BGMPlayer.Instance.Destroy();
                SceneManager.LoadScene(nextScene);
            });
        }

        var rot = Input.GetAxisRaw("KeyMoveX") + 2.0f*playerIcon.Sum(p => -p.Joycon?.GetStick()[0] ?? 0.0f);

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
