using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{

    public float myscore;
    private float mySclZ;
    public RawImage myMedal;
    public bool endGrowup;
    public Text txtScore;
    private float countScore;

	// Use this for initialization
	void Start ()
    {
        mySclZ = transform.localScale.z;
        endGrowup = false;
        countScore = 0;

    }
	
	// Update is called once per frame
	void Update ()
    {
       
		if(transform.localScale.z <= myscore*5)
        {
            mySclZ += 2.5f;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, mySclZ);
        }
        else
        {
            endGrowup = true;
        }
        if(countScore < myscore)
        {
            countScore += 0.5f;

        }
        if (countScore % 2 == 0)
        {
            CountScore();
        }

    }

    public void GraphScore(int Score)
    {
        myscore = Score;
    }

    public void CountScore()
    {
        txtScore.text = countScore.ToString();
    }
}
