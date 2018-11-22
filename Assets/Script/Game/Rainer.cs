using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = RainerLib.Tree;

public class Rainer : MonoBehaviour {

    public float rotateThreshold = 0.1f;
    public GameObject cloudPrefab;
    protected IEnumerator growTreeCoroutine;

    public virtual int PlayerNo { get; protected set; }
    public Transform Model { get; protected set; }
    public Transform MinimapIcon { get; protected set; }
    public CharacterController CharacterController { get; protected set; }
    public Animator Animator { get; protected set; }
    public Cloud Cloud { get; protected set; }



    // Use this for initialization
    protected virtual void Awake () {
        Model = transform.Find("model");
        MinimapIcon = transform.Find("minimapIcon");
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
	}

    protected virtual void Start()
    {

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

    protected virtual void OnDestroy()
    {
        DestroyCloud();
    }

    protected virtual void OnDisable()
    {
        if (gameObject.activeInHierarchy)
        {
            Animator.SetFloat("speed", 0.0f);
        }
    }

    public void CreateCloud(bool enableRainRayCast = false)
    {
        if (Cloud != null)
            return;

        Cloud = Instantiate(cloudPrefab, transform.parent).GetComponent<Cloud>();
        Cloud.target = transform;
        Cloud.enabled = true;
        Cloud.GetComponentInChildren<RainRayCast>().enabled = enableRainRayCast;

    }

    public void DestroyCloud()
    {
        if (Cloud != null)
        {
            Destroy(Cloud.gameObject);
        }
    }

    public Tree FindTree()
    {
        RaycastHit hitInfo;
        if (!Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return null;
        }

        return Ground.Instance.GetTree(hitInfo.textureCoord, PlayerNo);
    }

    public void StartGrowTree(Tree tree)
    {
        if (growTreeCoroutine != null || tree == null)
        {
            return;
        }

        growTreeCoroutine = GrowTree(tree);
        StartCoroutine(growTreeCoroutine);
    }

    public void StopGrowTree()
    {
        if (growTreeCoroutine == null)
            return;

        StopCoroutine(growTreeCoroutine);
        growTreeCoroutine = null;
    }

    private IEnumerator GrowTree(Tree tree)
    {
        yield return null;

        while (!tree.IsEndGrow)
        {
            tree.Grow(PlayerNo);
            yield return null;
        }

        growTreeCoroutine = null;
    }
}
