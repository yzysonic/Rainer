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
        FadeIn,
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

    public string nextScene = "TitleScene";

    [SerializeField]
    private List<GameObject> players = new List<GameObject>();

    [SerializeField]
    private List<GameObject> cameras = new List<GameObject>();

    [SerializeField]
    private List<GameObject> canvas = new List<GameObject>();

    [SerializeField]
    private StartLogo startLogo;

    [SerializeField]
    private MoveRange moveRange;

    public State startState;

    public PlayerCountSet playerCountSet;

    private State currentState;
    private int playerCount;
    private List<GameObject> activePlayers;
    private List<GameObject> activeCameras;
    private List<GameObject> activeCanvas;
    private GameTimer timer;

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
    public StartLogo StartLogo
    {
        get { return startLogo; }
    }
    public MoveRange MoveRange
    {
        get
        {
            return moveRange;
        }
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        timer = GameTimer.Instance;
        currentState = State.Init;
    }

    private void Start()
    {
        switch (CurrentState)
        {
            case State.Init:

                SetPlayerCount();
                SetPlayer();
                SetCamera();

                CurrentState = startState;
                return;

            case State.FadeIn:
                FadeInOut.Instance.FadeIn();
                CurrentState++;
                return;

            case State.CameraworkStart:
                activeCameras.ForEach(c => c.GetComponent<Animation>().Play("CameraStart"));
                return;

            case State.StartLogo:
                startLogo.gameObject.SetActive(true);
                startLogo.callback = () => CurrentState++;
                return;

            case State.Game:
                timer.gameObject.SetActive(true);
                RainerManager.Instance.enabled = true;
                activePlayers.ForEach(p => p.GetComponent<PlayerController>().enabled = true);
                activeCanvas.ForEach(c => c.gameObject.SetActive(true));
                return;

            case State.EndLogo:
                RainerManager.Instance.enabled = false;
                activePlayers.ForEach(p => p.GetComponent<PlayerController>().enabled = false);
                timer.enabled = false;
                ScoreManager.Instance.GetComponent<CircularGauge>().enabled = false;
                startLogo.GetComponent<Text>().text = "終了";
                startLogo.callback = () => CurrentState++;
                startLogo.gameObject.SetActive(true);
                return;

            case State.CameraworkEnd:
                timer.gameObject.SetActive(false);
                activeCanvas.ForEach(c => c.gameObject.SetActive(false));
                activeCameras.ForEach(c => c.GetComponent<CameraTopViewAnimation>().enabled = true);
                activeCameras.ForEach(c => c.GetComponent<CameraFallow>().enabled = false);
                BGMPlayer.Instance.Fade.Out();
                return;

            case State.EnterResult:
                var clipName = (playerCount > 2) ? "CameraEnterResult" : "CameraEnterResultTwoPlayer";
                cameras[0].GetComponent<Animation>().Play(clipName);
                BGMPlayer.Instance.Destroy();
                return;

            case State.Result:
                SceneManager.LoadSceneAsync("ResultScene", LoadSceneMode.Additive);
                activeCameras.GetRange(1, playerCount - 1).ForEach(c => c.SetActive(false));
                Ground.Instance.GrassField.enabled = true;
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
                    if (camera.GetComponent<CameraTopViewAnimation>().enabled)
                        return;
                }
                CurrentState++;
                return;

            case State.EnterResult:
                if (cameras[0].GetComponent<Animation>().isPlaying)
                    return;
                CurrentState++;
                return;

            case State.Result:
                if ((Input.GetButtonDown("Submit")) && !FadeInOut.Instance.enabled)
                {
                    FadeInOut.Instance.FadeOut(() => SceneManager.LoadScene(nextScene));
                }
                return;
        }
    }

    public void LateUpdate()
    {
        switch (CurrentState)
        {
            case State.Game:
                foreach (var player in activePlayers)
                {
                    var vPlayerToCenter = moveRange.transform.position - player.transform.position;
                    var distance = vPlayerToCenter.magnitude;
                    var diff = distance - moveRange.radius;
                    if (diff > 0.0f)
                    {
                        player.transform.Translate(vPlayerToCenter / distance * diff, Space.World);
                    }
                }
                break;
        }
    }

    #endregion

    #region Methods

    public void SetPlayerCount()
    {
        if (playerCountSet == PlayerCountSet.GetFromGameSetting)
            playerCount = GameSetting.PlayerCount;
        else
            playerCount = GameSetting.PlayerCount = (int)playerCountSet;
    }

    public void SetPlayer()
    {
        // プレイ人数分オブジェクトを有効にする
        for (var i = 0; i < players.Count; i++)
        {
            var active = i < playerCount;
            players[i].SetActive(active);
        }

        // 有効オブジェクトのリストを作成
        activePlayers = players.GetRange(0, playerCount);
        activeCanvas = canvas.GetRange(0, playerCount);

        // プレイヤーの関連オブジェクトの初期化
        if (Application.isPlaying)
        {
            var players = activePlayers.Select(p => p.GetComponent<PlayerController>()).ToList();
            foreach(var player in players)
            {
                player.CreateCloud(true);
                var rainer = RainerManager.Instance.SpawnRainer(player.transform.position + Vector3.right * 2.0f);
                rainer.SetFollow(player);
                rainer.enabled = false;
                player.AddStartAction(() => player.PushRainer(rainer));
            }
        }


    }

    public void SetCamera()
    {

        // プレイ人数分オブジェクトを有効にする
        for (var i = 0; i < cameras.Count; i++)
        {
            var active = i < playerCount;
            cameras[i].SetActive(active);
        }

        if (playerCount == 3)
        {
            cameras[4].SetActive(true);
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
