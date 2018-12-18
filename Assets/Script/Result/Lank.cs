using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lank : MonoBehaviour {

    class Data
    {
        public Graph graph;
        public float score;
        public RawImage medal;
        public Color medalColor;
        public int index;
        public PlayerController player;
        public bool isFinished;
    }

    public Texture[] texMedal;
    public Text txtWinner;
    public GameObject endLogo;
    public float graphTime;

    private Data[] dataList;
    private int playerCount;

    [HideInInspector] public float scoreTop;

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

            data.index = i + 1;

            data.medal = GameObject.Find("Medal" + data.index).GetComponent<RawImage>();
            data.medalColor = data.medal.color;
            data.medalColor.a = 0;
            data.medal.color = data.medalColor;

            data.graph = transform.Find("PlayerGraph" + data.index).GetComponent<Graph>();
            data.graph.GetComponent<Renderer>().material.color = Color.Lerp(GameSetting.PlayerColors[i], Color.white, 0.2f);
            data.graph.gameObject.SetActive(true);

            data.score = data.graph.myScore = ScoreManager.IsCreated ? ScoreManager.Instance.GetScore(i) : data.graph.myScore;

            data.player = GameSceneManager.Instance.PlayerList[i].GetComponent<PlayerController>();
            data.player.Model.transform.rotation = Quaternion.identity;
            data.player.DestroyCloud();
            data.player.transform.position = data.graph.transform.position + Vector3.up * 25;
            data.player.Animator.SetBool("fall", true);

            data.isFinished = false;
        }

        System.Array.Sort(dataList, (data1, data2) =>
        {
            if (data1.score < data2.score)
                return 1;
            if (data1.score > data2.score)
                return -1;

            return 0;
        });

        for (int i = 0; i < playerCount; i++)
        {
            Data data = dataList[i];

            data.medal.texture = texMedal[i];
        }

        for(int i = playerCount - 1; i > 0; i--)
        {
            while((int)dataList[i].graph.myScore >= (int) dataList[i-1].graph.myScore)
            {
                dataList[i - 1].score++;
                dataList[i - 1].graph.myScore++;
            }
        }

        scoreTop = dataList[0].score;
        FadeInOut.Instance.Alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerCount; i++)
        {
            Data data = dataList[i];

            data.player.CharacterController.SimpleMove(Vector3.zero);
            data.player.CharacterController.SimpleMove(Vector3.zero);

            if (data.player.CharacterController.velocity == Vector3.zero)
            {
                data.graph.readyGraph = true;
                data.player.Animator.SetBool("fall", false);
            }

            if (!data.isFinished)
            {
                if (data.graph.endGrowup)
                {
                    data.medalColor.a = 1;
                    data.medal.color = data.medalColor;
                    data.player.Animator.enabled = false;
                    data.player.Model.Rotate(Vector3.up * 180);

                    switch(i)
                    {
                        case 0:
                            data.player.GetComponentInChildren<Animation>().Play("Wizard_Run");

                            dataList[playerCount - 1].player.GetComponentInChildren<Animation>().Play("Wizard_Death");
                            BGMPlayer.Instance.AudioSources[1].Play();
                            endLogo.SetActive(true);
                            txtWinner.text = dataList[0].index + "Pのかち";

                            break;

                        default:
                            data.player.GetComponentInChildren<Animation>().Play("Wizard_Stun");

                            break;
                    }

                    data.isFinished = true;
                }
            }
        }

        if ((dataList[0].isFinished
            && Input.GetButtonDown("Submit")
            || JoyconManager.GetButtonDown(GameSetting.JoyconButton.Start))
            && !FadeInOut.Instance.enabled)
        {
            endLogo.GetComponent<Animation>().Play();
            BGMPlayer.Instance.Fade.Out();
            FadeInOut.Instance.FadeOut(() =>
            {
                if (Ground.IsCreated)
                {
                    Ground.Instance.Destroy();
                }
                BGMPlayer.Instance.Destroy();
                SceneManager.LoadScene("TitleScene");
            });
        }
    }
}
