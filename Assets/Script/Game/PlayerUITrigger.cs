using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = RainerLib.Tree;

[RequireComponent(typeof(SphereCollider))]
public class PlayerUITrigger : MonoBehaviour
{
    private PlayerUIManager uiManager;
    private Ground ground;

    public List<RainerController> NearRainers { get; private set; } = new List<RainerController>();
    public List<Tree> NearTrees { get; private set; } = new List<Tree>();
    public RainerController NearestRainer { get; private set; }
    public Tree NearestTree { get; private set; }

	// Use this for initialization
	void Start () {
        uiManager = transform.parent.GetComponent<PlayerController>().uiManager;
        ground = Ground.IsCreated ? Ground.Instance : null;
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
        NearestTree = FindNearest(NearTrees);

        if (!GameSceneManager.IsCreated)
        {
            return;
        }

        uiManager.UIGetRainer.Target = NearestRainer?.transform;
        if(uiManager.UIRainerCount.Value > 0)
        {
            int grassNo = ground.GrassField.GetPlayerNoOfGrass(ground.GetUV(transform.position));
            if (grassNo != -1
                && grassNo != GetComponentInParent<PlayerController>().PlayerNo)
            {
                uiManager.UIGrowTreeBubble.Target = null;
                uiManager.UIGrowTree.Target = transform;
            }
            else
            {
                uiManager.UIGrowTree.Target = null;
                uiManager.UIGrowTreeBubble.Target = transform;
            }
        }
        else
        {
            uiManager.UIGrowTree.Target = null;
            uiManager.UIGrowTreeBubble.Target = null;
        }

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
