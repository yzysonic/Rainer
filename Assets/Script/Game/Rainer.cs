using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = RainerLib.Tree;

public class Rainer : MonoBehaviour {

    public float rotateThreshold = 0.1f;
    public GameObject cloudPrefab;

    public virtual int PlayerNo { get; set; }
    public Transform Model { get; protected set; }
    public Transform MinimapIcon { get; protected set; }
    public CharacterController CharacterController { get; protected set; }
    public Animator Animator { get; protected set; }
    public Cloud Cloud { get; protected set; }
    public Renderer CoatRenderer { get; set; }
    public Color Color
    {
        get
        {
            return CoatRenderer.material.color;
        }
        set
        {
            CoatRenderer.material.color = value;
        }
    }




    // Use this for initialization
    protected virtual void Awake () {
        Model = transform.Find("model");
        MinimapIcon = Model.Find("minimapIcon");
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        CoatRenderer = Model.Find("Fantasy_Wizard_Cape").GetComponent<Renderer>();
    }

    protected virtual void Start()
    {

    }
	
	// Update is called once per frame
    public void UpdateModel () {
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
            Cloud = null;
        }
    }

    public Tree FindTree()
    {
        var uv = Ground.Instance.GetUV(transform.position - Model.forward * 3.0f);
        return Ground.Instance.GetTree(uv, PlayerNo);
    }

}
