using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;
using Tree = RainerLib.Tree;

public class SeedIcon : MonoBehaviour {

    public float interval = 0.7f;
    public float distance = 1.0f;
    public float speed = 10.0f;
    public float maxHeight = 0.3f;
    public float offsetHeight = 3.86f;
    public float offsetWidth = 1.7f;
    public Color completeColor = Color.green;

    private Tree tree;
    private Vector3 pos;
    private float initHeight;
    private Timer timer;
    private Transform imgBase;
    private CircularGauge circularGauge;
    private Color originColor;

	// Use this for initialization
	void Start () {
        pos = transform.localPosition;
        initHeight = pos.y;
        timer = new Timer(interval);
        tree = transform.parent.parent.Find("Tree").GetComponent<Tree>();
        imgBase = transform.Find("Base");
        circularGauge = GetComponentInChildren<CircularGauge>();
        originColor = circularGauge.Colors[0];
    }
	
	// Update is called once per frame
	void Update () {
        var treeScale = tree.transform.localScale.y;
        if (treeScale > 0.0f)
        {
            if (treeScale < maxHeight)
            {
                pos.y = Mathf.Lerp(initHeight, offsetHeight, treeScale / maxHeight);
                pos.x = Mathf.Lerp(pos.x, 0.0f, 7.0f*Time.deltaTime);
            }
            else
            {
                pos.x = Mathf.Lerp(pos.x, offsetWidth, 7.0f*Time.deltaTime);
            }
            imgBase.localRotation = Quaternion.Euler(0.0f, 0.0f, -Mathf.Atan2(imgBase.parent.localPosition.x, imgBase.parent.localPosition.y)*Mathf.Rad2Deg);
        }
        else
        {
            timer++;

            if (timer.TimesUp())
            {
                pos.y = initHeight;
                timer.Reset(interval);
            }

            pos.y = Mathf.Lerp(pos.y, initHeight + distance, speed * Time.deltaTime);
            transform.localPosition = pos;

        }
        transform.localPosition = pos;
        circularGauge.Values[0] = tree.Progress;
        circularGauge.Values[1] = 1.0f-tree.Progress;

        if (tree.Progress >= 1.0f)
        {
            circularGauge.Colors[0] = Color.Lerp(originColor, completeColor, tree.Progress);
            circularGauge.Colors = circularGauge.Colors;
            enabled = false;
        }
    }
}
