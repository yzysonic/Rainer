using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

[RequireComponent(typeof(CircularGauge))]
public class ScoreManager : Singleton<ScoreManager>
{
    public float showPeriod = 10.0f;
    public float showInterval = 3.0f;

    private int[] scores = new int[4];
    private CircularGauge circularGauge;
    private Timer timer;

    // Use this for initialization
    public void Start () {
        circularGauge = GetComponent<CircularGauge>();
        InitGauge();
        GameTimer.Instance.AddPeriodEvent(showPeriod, ShowScoreGauge);
    }

    public int GetScore(int playerNo)
    {
        return scores[playerNo];
    }

    public void AddScore(int playerNo, int point)
    {
        scores[playerNo] += point;

        int index = 0;
        switch(GameSetting.PlayerCount)
        {
            case 2:
                switch (playerNo)
                {
                    case 0: index = 1; break;
                    case 1: index = 0; break;
                }
                break;

            case 3:
                switch (playerNo)
                {
                    case 0: index = 2; break;
                    case 1: index = 0; break;
                    case 2: index = 1; break;
                }
                break;

            case 4:
                switch(playerNo)
                {
                    case 0: index = 3; break;
                    case 1: index = 0; break;
                    case 2: index = 2; break;
                    case 3: index = 1; break;
                }
                break;
        }

        circularGauge.Values[index] = scores[playerNo];
    }

    public void ShowScoreGauge()
    {
        if (circularGauge.enabled)
            return;

        circularGauge.enabled = true;
        GameTimer.Instance.AddPeriodEvent(showInterval, () => circularGauge.enabled = false, 1);
    }

    public void InitGauge()
    {
        var playerCount = GameSetting.PlayerCount;
        circularGauge.Division = playerCount;
        var colors = new Color[playerCount];

        switch (playerCount)
        {
            case 2:
                colors[0] = GameSetting.PlayerColors[1];
                colors[1] = GameSetting.PlayerColors[0];
                break;

            case 3:
                colors[0] = GameSetting.PlayerColors[1];
                colors[1] = GameSetting.PlayerColors[2];
                colors[2] = GameSetting.PlayerColors[0];
                break;

            case 4:
                colors[0] = GameSetting.PlayerColors[1];
                colors[1] = GameSetting.PlayerColors[3];
                colors[2] = GameSetting.PlayerColors[2];
                colors[3] = GameSetting.PlayerColors[0];
                break;
        }

        circularGauge.Colors = colors;
        circularGauge.UpdateGauge();

    }
}
