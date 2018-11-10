    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Medal : MonoBehaviour {

    public float medalRot;
    public float testRot;
    public bool checkRot;
    //private bool graphEnd1, graphEnd2, graphEnd3, graphEnd4;


    // Use this for initialization
    void Start () {

        medalRot = transform.rotation.y;
        checkRot = true;


    }

    // Update is called once per frame
    void Update () {

        if (transform.rotation.y >= 0.7f)
        {
            checkRot = false;
        }
        else if(transform.rotation.y <= -0.7f)
        {
            checkRot = true;
        }

        if (checkRot)
        {
            medalRot = 45f * Time.deltaTime;
        }
        else
        {
            medalRot = -45f * Time.deltaTime;
        }
        testRot = transform.rotation.y;
        transform.Rotate(0, medalRot, 0,Space.World);
	}
}
