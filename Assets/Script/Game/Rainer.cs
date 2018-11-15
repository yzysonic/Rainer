using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainer : MonoBehaviour {

    public float rotateThreshold = 0.1f;
    protected IEnumerator growTree;

    public virtual int PlayerNo { get; protected set; }
    public Transform Model { get; protected set; }
    public CharacterController CharacterController { get; protected set; }
    public Animator Animator { get; protected set; }


    // Use this for initialization
    protected virtual void Start () {
        Model = transform.Find("model");
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        var velocity = new Vector3(-CharacterController.velocity.x, 0.0f, -CharacterController.velocity.z);
        var speed = velocity.magnitude;

        // アニメーションのパラメータ設定
        Animator.SetFloat("speed", speed);

        // モデルの向きを設定
        if(speed > rotateThreshold)
        {
            Model.rotation = Quaternion.LookRotation(velocity);
        }

    }

    protected virtual void OnDisable()
    {
        if (gameObject.activeInHierarchy)
        {
            Animator.SetFloat("speed", 0.0f);
        }
    }

    public void StartGrowTree()
    {
        if (growTree != null)
        {
            return;
        }

        RaycastHit hitInfo;
        if (!Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return;
        }

        var tree = Ground.Instance.GetTree(hitInfo.textureCoord, PlayerNo);
        if (tree == null)
        {
            return;
        }

        growTree = GrowTree(tree);
        StartCoroutine(growTree);
    }

    public void StopGrowTree()
    {
        if (growTree == null)
            return;

        StopCoroutine(growTree);
        growTree = null;
    }

    private IEnumerator GrowTree(Tree tree)
    {
        yield return null;

        while (!tree.IsEndGrow)
        {
            tree.Grow(PlayerNo);
            yield return null;
        }

        growTree = null;
    }
}
