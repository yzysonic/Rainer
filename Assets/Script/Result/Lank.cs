using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lank : MonoBehaviour {

    class Data
    {
        public GameObject player;
        public Graph graph;
        public float score;
        public RawImage medal;
        public bool endGrowup;
        public string setWinner;
    }


    private RawImage endLogo;
    public Texture []texMedal;
    private Data[] dataList;
    private int playerCount;
    public int graphFrame;
    public bool IsFinish { get; private set; }

    [HideInInspector] public string setWinner;
    [HideInInspector] public float scoreTop;
    [HideInInspector] public float scoreUnit;


    private void Awake()
    {
        playerCount = GameSetting.PlayerCount;
    }

    // Use this for initialization
    void Start () {

        dataList = new Data[playerCount];

        for (int i = 0; i < playerCount; i++)
        {
            Data data = dataList[i] = new Data();

            data.player = transform.Find("Player" + (i+1)).gameObject;
            data.player.GetComponent<Renderer>().material.color = GameSetting.PlayerColors[i];

            data.medal = GameObject.Find("Medal" + (i+1)).GetComponent<RawImage>();

            data.graph = data.player.GetComponent<Graph>();

            data.graph.gameObject.SetActive(true);

            data.score = data.graph.myscore = ScoreManager.IsCreated ? ScoreManager.Instance.GetScore(i) : data.graph.myscore;

            data.setWinner = (i+1) +"P WIN";

            data.medal.color = new Color(data.medal.color.r, data.medal.color.g, data.medal.color.b, 0);
            
        }

        endLogo = GameObject.Find("EndLogo").GetComponent<RawImage>();

        FadeInOut.Instance.Alpha = 0.0f;
    }

    // Update is called once per frame
    void Update () {

        for (int i = 0; i < playerCount; i++)
        {
            dataList[i].endGrowup = dataList[i].graph.endGrowup;
        }

        System.Array.Sort(dataList, (data1, data2) =>
        {
            if (data1.score > data2.score)
                return 1;
            if (data1.score < data2.score)
                return -1;

            return 0;
        });


        for (int i = 0; i < playerCount; i++)
        {
            dataList[i].medal.texture = texMedal[i + (4 - playerCount)];
        }

        if (dataList[playerCount - 1].endGrowup)
        {
            setWinner = dataList[playerCount - 1].setWinner;
        }

        scoreTop = dataList[playerCount - 1].score;
        scoreUnit = dataList[playerCount - 1].score / graphFrame;

        if (!IsFinish)
        {
            IsFinish = dataList[playerCount - 1].endGrowup;
            if (IsFinish)
            {
                BGMPlayer.Instance.AudioSources[1].Play();
                endLogo.color = new Color(1, 1, 1, 1);
            }
        }

        myAlpha();


        if ((Input.GetButtonDown("Submit") || JoyconManager.GetButtonDown(GameSetting.JoyconButton.Start)) && !FadeInOut.Instance.enabled && IsFinish)
        {
            endLogo.GetComponent<Animation>().Play();
            BGMPlayer.Instance.Fade.Out();
            FadeInOut.Instance.FadeOut(() => {
                if (Ground.IsCreated)
                {
                    Ground.Instance.Destroy();
                }
                BGMPlayer.Instance.Destroy();
                SceneManager.LoadScene("TitleScene");
            });
        }

    }

    void myAlpha()
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (dataList[i].endGrowup)
            {
                dataList[i].medal.color = new Color(dataList[i].medal.color.r, dataList[i].medal.color.g, dataList[i].medal.color.b, 1);
            }
        }
    }

}
