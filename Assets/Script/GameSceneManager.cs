using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    private bool hasPlayerSetted;
    private int numPlayer;

    [SerializeField]
    private List<GameObject> players = new List<GameObject>();

    [SerializeField]
    private List<GameObject> cameras = new List<GameObject>();

    private void Awake()
    {
    }

    private void Start()
    {
        SetPlayerAndCamera();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void SetPlayerAndCamera()
    {
        var numPlayer = GameSetting.NumPlayer;

        for(var i = 0; i < 4; i++)
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
