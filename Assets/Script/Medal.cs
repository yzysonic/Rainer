    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Medal : MonoBehaviour {

    public float medalScl;
    public AnimationCurve curve;
    private bool graphEnd1, graphEnd2, graphEnd3, graphEnd4;


    // Use this for initialization
    void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {

        //medalScl = curve.Evaluate(Time.time / 2f);
        //transform.localScale = new Vector3(medalScl, medalScl, medalScl);
        transform.Rotate(0,45f * Time.deltaTime, 0,Space.World);
	}
}
