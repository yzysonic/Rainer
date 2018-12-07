using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Rainer {

    public float speed = 1.0f;
    public int playerNo;

    protected override void Awake()
    {
        base.Awake();
        PlayerNo = playerNo;

    }
    // Use this for initialization
    protected override void Start () {

        base.Start();
        CreateCloud(true);
    }

    // Update is called once per frame
    protected void Update () {
        CharacterController.SimpleMove(Vector3.left * speed);
        UpdateModel();
    }
}
