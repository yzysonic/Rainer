using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{

    [SerializeField]
    private List<GameObject> players = new List<GameObject>();

    [SerializeField]
    private List<GameObject> cameras = new List<GameObject>();

    [SerializeField]
    private List<GameObject> canvas = new List<GameObject>();

    [SerializeField]
    private StartLogo startLogo;

    [SerializeField]
    private Timer timer;

    private bool hasPlayerSetted;
    private int numPlayer;
    private int state;

    private void Start()
    {
        SetPlayer();
        startLogo.Active = true;
        state = 0;
    }

    public void Update()
    {
        switch (state)
        {
            case 0:
                // カメラワーク
                state++;
                break;

            case 1:
                if (startLogo.Active != false)
                    break;

                timer.enabled = true;
                players.ForEach(p => p.GetComponent<PlayerController>().enabled = true);
                state++;
                break;


            case 2:

                if (timer.RemainingTime <= 0.0f)
                {
                    timer.enabled = false;
                    startLogo.Text = "終了";
                    startLogo.Active = true;
                    state++;
                }
                break;

            case 3:
                // カメラワーク
                state++;
                break;
        }
    }

    public void SetPlayer()
    {
        numPlayer = GameSetting.NumPlayer;

        for (var i = 0; i < 4; i++)
        {
            var active = i < numPlayer;
            players[i].SetActive(active);
            cameras[i].SetActive(active);
            canvas[i].SetActive(active);
        }

        // カメラの個別設定
        var cameraComs = cameras.Select(c => c.GetComponent<Camera>()).ToList();
        switch (numPlayer)
        {
            case 1:
                cameraComs[0].rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                break;

            case 2:
                cameraComs[0].rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
                cameraComs[1].rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
                break;

            case 3:
                cameraComs[0].rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
                cameraComs[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                cameraComs[2].rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
                break;

            case 4:
                cameraComs[3].rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
                goto case 3;
        }

    }
}
