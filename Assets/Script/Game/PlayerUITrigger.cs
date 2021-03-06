﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = RainerLib.Tree;

[RequireComponent(typeof(SphereCollider))]
public class PlayerUITrigger : MonoBehaviour
{
    private PlayerUIManager uiManager;

    public List<RainerController> NearRainers { get; private set; } = new List<RainerController>();
    public List<Tree> NearTrees { get; private set; } = new List<Tree>();
    public RainerController NearestRainer { get; private set; }
    public Tree NearestTree { get; private set; }

	// Use this for initialization
	void Start () {
        uiManager = transform.parent.GetComponent<PlayerController>().uiManager;
	}

    private void Update()
    {
        for (var i = 0; i < NearTrees.Count;)
        {
            if (NearTrees[i] == null)
            {
                NearTrees.RemoveAt(i);
                continue;
            }

            i++;
        }

        NearestRainer = FindNearest(NearRainers);

        if(uiManager?.UIGetRainer != null)
        {
            uiManager.UIGetRainer.Target = NearestRainer?.transform;
        }

        NearestTree = FindNearest(NearTrees);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Rainer")
        {
            var rainer = other.GetComponent<RainerController>();
            NearRainers.Add(rainer);
        }
        else if(other.tag == "Tree")
        {
            var tree = other.GetComponent<Tree>();

            if(tree.PlayerNo < 0)
            {
                NearTrees.Add(tree);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Rainer")
        {
            RemoveFromList(NearRainers, other.gameObject);
        }
        else if(other.tag == "Tree")
        {
            RemoveFromList(NearTrees, other.gameObject);
        }
    }

    public T FindNearest<T>(List<T> list) where T : MonoBehaviour
    {
        var minSqrDistance = Mathf.Infinity;
        T nearest = null;

        foreach (var obj in list)
        {
            var sqrDistance = (transform.position - obj.transform.position).sqrMagnitude;
            if (sqrDistance >= minSqrDistance)
            {
                continue;
            }

            minSqrDistance = sqrDistance;
            nearest = obj;
        }

        return nearest;
    }

    public static void RemoveFromList<T>(List<T> list, GameObject gameObject) where T : MonoBehaviour
    {
        var index = list.FindIndex(t => t.gameObject == gameObject);
        if (index >= 0)
        {
            list.RemoveAt(index);
        }
    }
}
