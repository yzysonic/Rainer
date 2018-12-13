using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{

    public float myScore;
    private Vector3 myScale;
    public RawImage myMedal;
    public bool endGrowup;
    public Text txtScore;
    private float countScore;
    public Lank lank;
    private float growSpeed;


	// Use this for initialization
	void Start ()
    {
        myScale = transform.localScale;
        endGrowup = false;
        countScore = 0;
        lank = GetComponentInParent<Lank>();
        growSpeed = 4.75f / lank.graphFrame;
    }
	
    // Update is called once per frame
    void Update ()
    {
        if(transform.localScale.z <= (myScore / lank.scoreTop) * 4.75f)
        {
            myScale.z += growSpeed;

            transform.localScale = myScale;
        }
        else
        {
            endGrowup = true;
        }

        if(countScore < myScore)
        {
            countScore += lank.scoreTop / lank.graphFrame;
        }
        else
        {
            countScore = myScore;
        }
        
        txtScore.text = ((int)countScore).ToString();
    }
}
