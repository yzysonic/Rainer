using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class Tree : MonoBehaviour {

    public int growScore = 2;
    public int bonusScore = 50;
    public float scoreTime = 0.5f;
    public float growTime = 10;
    private Timer scoreTimer;
    private Timer bonusTimer;
    private Vector3 treeMaxScl;
    private bool isEndGrow;
    

	// Use this for initialization
	void Start () {

        scoreTimer = new Timer(scoreTime);
        bonusTimer = new Timer(growTime);
        treeMaxScl = transform.localScale;
        transform.localScale = Vector3.zero;
        isEndGrow = false;

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.F))
        {
            Grow(0);
        }
    }

    public void Grow(int playerNo)
    {
        if (isEndGrow)
        {
            return;
        }

        bonusTimer.Step();
        scoreTimer.Step();
        transform.localScale = Vector3.Lerp(Vector3.zero, treeMaxScl, bonusTimer.Progress);

        if (scoreTimer.TimesUp())
        {
            scoreTimer.Reset();
            //ScoreManager.Instance.AddScore(playerNo, growScore);
            //スコア更新

        }

        if (bonusTimer.TimesUp())
        {
            //ボーナススコア更新
            //ScoreManager.Instance.AddScore(playerNo, bonusScore);
            isEndGrow = true;
        }
        
    }
}
