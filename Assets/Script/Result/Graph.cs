using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{

    public float myScore;
    private Vector3 myScale;
    public RawImage myMedal;
    public bool readyGraph;
    public bool endGrowup;
    public Text txtScore;
    private float countScore;
    public Lank lank;
    private float growSpeed;


	// Use this for initialization
	void Start ()
    {
        myScale = transform.localScale;
        readyGraph = false;
        endGrowup = false;
        countScore = 0;
        lank = GetComponentInParent<Lank>();
        growSpeed = -0.03f;
    }
	
    // Update is called once per frame
    void Update ()
    {
        if (readyGraph)
        {
            if (!endGrowup)
            {
                myScale.z += growSpeed;
                growSpeed += 0.0009f;
                transform.localScale = myScale;

                if (growSpeed >= 0
                    && transform.localScale.z >= (myScore / lank.scoreTop) * 4.75f)
                {
                    myScale.z = (myScore / lank.scoreTop) * 4.75f;
                    transform.localScale = myScale;

                    txtScore.text = ((int)myScore).ToString();
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
                myScale.z += 0.025f;

                transform.localScale = myScale;
            }
        }
    }
}
