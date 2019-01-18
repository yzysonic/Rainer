using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RainerLib;
using System;

public class Graph : MonoBehaviour
{

    public float myScore;
    public RawImage myMedal;
    public bool readyGraph;
    public bool endGrowup;
    public Text txtScore;
    public Lank lank;
    public float percentage;

    private Vector3 myScale;
    //private float countScore;
    private Timer timer;


    // Use this for initialization
    void Start ()
    {
        myScale = transform.localScale;
        readyGraph = false;
        endGrowup = false;
        //countScore = 0;
        lank = GetComponentInParent<Lank>();
        timer = new Timer(lank.graphTime);
    }
	
    // Update is called once per frame
    void Update ()
    {
        if (readyGraph)
        {
            if (!endGrowup)
            {
                timer++;
                myScale.z = 8 * (timer.Progress - 0.25f) * (timer.Progress - 0.25f);
                transform.localScale = myScale;

                if (timer.Progress > 0.25f)
                {
                    percentage = (myScore / lank.sumScore) * (myScale.z / ((myScore / lank.scoreTop) * 4.5f));
                    //percentage = (float)Math.Round(percentage, 4);
                    txtScore.text = $"{(Math.Round(percentage, 3) * 100):f1}%";
                }

                if (timer.Progress > 0.25f
                    && myScale.z >= (myScore / lank.scoreTop) * 4.5f)
                {
                    myScale.z = (myScore / lank.scoreTop) * 4.5f;
                    transform.localScale = myScale;

                    percentage = (myScore / lank.sumScore) * (myScale.z / ((myScore / lank.scoreTop) * 4.5f));
                    percentage = (float)Math.Round(percentage, 4);
                    txtScore.text = $"{(Math.Round(percentage, 3) * 100):f1}%";
                    endGrowup = true;
                }
            }
            //if (countScore < myScore)
            //{
            //    countScore += lank.scoreTop / lank.graphFrame;
            //}
            //else
            //{
            //    countScore = myScore;
            //}
        }
        else
        {
            if (transform.localScale.z < 0.5f)
            {
                myScale.z += 0.05f;

                transform.localScale = myScale;
            }
        }
    }
}
