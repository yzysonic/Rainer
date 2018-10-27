using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RainerLib;

public class GameSceneManager : Singleton<GameSceneManager>
{
    public enum State
    {
        Init,
        CameraworkStart,
        StartLogo,
        Game,
        EndLogo,
        CameraworkEnd,
        Result
    }

    public State startState = State.Init;

    [SerializeField]
    private List<GameObject> players = new List<GameObject>();

    [SerializeField]
    private List<GameObject> cameras = new List<GameObject>();

    [SerializeField]
    private List<GameObject> canvas = new List<GameObject>();

    [SerializeField]
    private StartLogo startLogo;

    [SerializeField]
    private GameTimer timer;

    [SerializeField]
    private ScoreManager scoreManager;

    private int numPlayer;
    private State currentState;

    public State CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            if(currentState != value)
            {
                currentState = value;
                Start();
            }
        }
    }

    private void Start()
    {
        switch (CurrentState)
        {
            case State.Init:
                SetPlayer();
                CurrentState++;
                break;

            case State.CameraworkStart:
                break;

            case State.StartLogo:
                startLogo.gameObject.SetActive(true);
                startLogo.callback = () => CurrentState++;
                break;

            case State.Game:
                timer.gameObject.SetActive(true);
                players.TakeWhile(p => p.activeSelf)
                    .ToList()
                    .ForEach((p) => {
                        var controller = p.GetComponent<PlayerController>();
                        controller.enabled = true;
                        controller.canvas.gameObject.SetActive(true);
                    });
                break;

            case State.EndLogo:
                startLogo.GetComponent<Text>().text = "終了";
                startLogo.callback = () => CurrentState++;
                startLogo.gameObject.SetActive(true);
                break;
        }

    }

    public void Update()
    {
        switch (CurrentState)
        {
            case State.CameraworkStart:
                CurrentState++;
                break;

            case State.Game:
                if (timer.TimesUp)
                {
                    timer.enabled = false;
                    CurrentState++;
                }
                break;

            case State.CameraworkEnd:
                CurrentState++;
                break;

            default: return;
        }
    }

    public void SetPlayer()
    {
        numPlayer = GameSetting.PlayerCount;

        for (var i = 0; i < 4; i++)
        {
            var active = i < numPlayer;
            players[i].SetActive(active);
            cameras[i].SetActive(active);
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
