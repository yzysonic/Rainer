﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Rainer {

    public float speed = 1.0f;
    public int playerNo;

    private void Awake()
    {
        PlayerNo = playerNo;

    }
    // Use this for initialization
    protected override void Start () {

        base.Start();

    }

    // Update is called once per frame
    protected override void Update () {
        CharacterController.SimpleMove(Vector3.left * speed);
        base.Update();
    }
}
