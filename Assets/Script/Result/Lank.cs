using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    public Texture []texMedal;
    private Data[] dataList;
    private int playerCount;
    public string setWinner;


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

            data.medal = GameObject.Find("Medal" + (i+1)).GetComponent<RawImage>();

            data.graph = data.player.GetComponent<Graph>();

            data.graph.gameObject.SetActive(true);

            data.score = data.graph.myscore;

            data.setWinner = (i+1) +"P WIN";

            data.medal.color = new Color(data.medal.color.r, data.medal.color.g, data.medal.color.b, 0);

        }

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
        myAlpha();
    }

    void myAlpha()
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (dataList[i].endGrowup)
            {
                dataList[i].medal.color = new Color(dataList[i].medal.color.r, dataList[i].medal.color.g, dataList[i].medal.color.b, 255);
            }
        }
    }
}
