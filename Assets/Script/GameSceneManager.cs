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
        CameraworkStart,
        StartLogo,
        Game,
        EndLogo,
        CameraworkEnd,
        EnterResult,
        Result,
    }

    public enum PlayerCountSet
    {
        GetFromGameSetting,
        TwoPlayer = 2,
        ThreePlayer,
        FourPlayer,
    }

    #region Fields

    public State startState;

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

    [SerializeField]
    private PlayerCountSet playerCountSet;

    private State currentState;
    private int playerCount;
    private List<GameObject> activePlayers;
    private List<GameObject> activeCameras;
    private List<GameObject> activeCanvas;

    #endregion

    #region Properties

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
    public List<GameObject> PlayerList
    {
        get
        {
            return new List<GameObject>(players);
        }
    }
    public List<GameObject> CameraList
    {
        get
        {
            return new List<GameObject>(cameras);
        }
    }
    public List<GameObject> CanvasList
    {
        get
        {
            return new List<GameObject>(canvas);
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        SetPlayerCount();
        SetPlayer();
        SetCamera();
        currentState = startState;
    }

    private void Start()
    {
        switch (CurrentState)
        {

            case State.CameraworkStart:
                activeCameras.ForEach(c => c.GetComponent<Animation>().Play("CameraStart"));
                return;

            case State.StartLogo:
                startLogo.gameObject.SetActive(true);
                startLogo.callback = () => CurrentState++;
                return;

            case State.Game:
                timer.gameObject.SetActive(true);
                activePlayers.ForEach(p => p.GetComponent<PlayerController>().enabled = true);
                activeCanvas.ForEach(c => c.gameObject.SetActive(true));
                return;

            case State.EndLogo:
                activePlayers.ForEach(p => p.GetComponent<PlayerController>().enabled = false);
                timer.enabled = false;
                startLogo.GetComponent<Text>().text = "終了";
                startLogo.callback = () => CurrentState++;
                startLogo.gameObject.SetActive(true);
                return;

            case State.CameraworkEnd:
                timer.gameObject.SetActive(false);
                activeCanvas.ForEach(c => c.gameObject.SetActive(false));
                activeCameras.ForEach(c => c.GetComponent<Animation>().Play("CameraEnd"));
                return;

            case State.EnterResult:
                cameras[0].GetComponent<Animation>().Play("CameraEnterResult");
                return;
        }

    }

    public void Update()
    {
        switch (CurrentState)
        {
            case State.CameraworkStart:
                foreach(var camera in activeCameras)
                {
                    if (camera.GetComponent<Animation>().isPlaying)
                        return;
                }
                CurrentState++;
                return;

            case State.Game:
                if (timer.TimesUp)
                {
                    CurrentState++;
                }
                return;

            case State.CameraworkEnd:
                foreach (var camera in activeCameras)
                {
                    if (camera.GetComponent<Animation>().isPlaying)
                        return;
                }
                CurrentState++;
                return;

            case State.EnterResult:
                if (cameras[0].GetComponent<Animation>().isPlaying)
                    return;
                CurrentState++;
                return;
        }
    }

    #endregion

    #region Methods

    public void SetPlayerCount()
    {
        if (playerCountSet == PlayerCountSet.GetFromGameSetting)
            playerCount = GameSetting.PlayerCount;
        else
            playerCount = (int)playerCountSet;
    }

    public void SetPlayer()
    {
        // プレイ人数分オブジェクトを有効にする
        for (var i = 0; i < 4; i++)
        {
            var active = i < playerCount;
            players[i].SetActive(active);
        }

        // 有効オブジェクトのリストを作成
        activePlayers = new List<GameObject>();
        activeCanvas = new List<GameObject>();

        for (var i=0;i<playerCount;i++)
        {
            activePlayers.AddRange(players.GetRange(0, playerCount));
            activeCanvas.AddRange(canvas.GetRange(0, playerCount));
        }

    }

    public void SetCamera()
    {

        // プレイ人数分オブジェクトを有効にする
        for (var i = 0; i < 4; i++)
        {
            var active = i < playerCount;
            cameras[i].SetActive(active);
        }

        // 有効オブジェクトのリストを作成
        activeCameras = new List<GameObject>();

        for (var i = 0; i < playerCount; i++)
        {
            activeCameras.AddRange(cameras.GetRange(0, playerCount));
        }

        SetCameraRect();

    }

    public void SetCameraRect()
    {
        var cameraComs = cameras.Select(c => c.GetComponent<Camera>()).ToList();
        switch (GameSetting.PlayerCount)
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


    #endregion

}
