using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircularGage))]
public class ScoreManager : MonoBehaviour
{
    private int[] scores = new int[4];

    private CircularGage circularGage;

    // Use this for initialization
    public void Start () {
        circularGage = GetComponent<CircularGage>();
        circularGage.Division = GameSetting.NumPlayer;
        circularGage.Init();
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
        switch(GameSetting.NumPlayer)
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

        circularGage.Values[index] = scores[playerNo];
    }
}
