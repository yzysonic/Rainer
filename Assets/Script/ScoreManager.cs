using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircularGauge))]
public class ScoreManager : MonoBehaviour
{
    private int[] scores = new int[4];

    private CircularGauge circularGauge;

    // Use this for initialization
    public void Start () {
        circularGauge = GetComponent<CircularGauge>();
        circularGauge.Division = GameSetting.PlayerCount;
        circularGauge.UpdateGauge();
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
