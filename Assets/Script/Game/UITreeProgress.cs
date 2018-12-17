using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;
using Tree = RainerLib.Tree;

public class UITreeProgress : UIFallow
{
    private Tree tree;

    public CircularGauge CircularGauge { get; private set; }


    // Use this for initialization
    protected override void Awake () {
        base.Awake();
        CircularGauge = transform.Find("Guage").GetComponent<CircularGauge>();
    }

    private void OnEnable()
    {
        tree = Target.GetComponent<Tree>();
        if(tree == null)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
        CircularGauge.Values[0] = tree.Progress;
        CircularGauge.Values[1] = 1.0f-tree.Progress;
	}
}
