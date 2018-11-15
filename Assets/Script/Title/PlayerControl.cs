using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Rainer {

	// Use this for initialization
	protected override void Start () {
        base.Start();

	}

    // Update is called once per frame
    protected override void Update () {
        CharacterController.SimpleMove(Vector3.left);
        base.Update();
    }
}
