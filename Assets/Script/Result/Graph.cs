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
    public Lank lank;
    private float growSpeed;


	// Use this for initialization
	void Start ()
    {
        mySclZ = transform.localScale.z;
        endGrowup = false;
        countScore = 0;
        lank = GetComponentInParent<Lank>();
        growSpeed = 500.0f / lank.graphFrame;

    }
	
	// Update is called once per frame
	void Update ()
    {
       
		if(transform.localScale.z <= (myscore / lank.scoreTop) *500)
        {
            mySclZ += growSpeed;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, mySclZ);
        }
        else
        {
            endGrowup = true;
        }
        if(countScore < myscore)
        {
            countScore += lank.scoreUnit;
        }
        else
        {
            countScore = myscore;
        }
        CountScore();

    }

    public void CountScore()
    {
        int viewScore = (int)countScore;
        txtScore.text = viewScore.ToString();
    }
}
