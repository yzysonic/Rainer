using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainRayCast : MonoBehaviour {

    private RaycastHit hitInfo;
    private GrassSpawn grassSpawn;
    private Ground ground;
    private float timer;
    private int layerMask;
    private int playerNo;

    // Use this for initialization
    void Start () {
        //grassSpawn = GameObject.Find("Ground").GetComponent<GrassSpawn>();
        ground = GameObject.Find("Ground").GetComponent<Ground>();
        timer = 0.0f;
        layerMask = LayerMask.GetMask("Ground");
        playerNo = transform.parent.GetComponent<Cloud>().target.GetComponent<PlayerController>().PlayerNo;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        timer += Time.deltaTime;
        //if (timer <= 0.6f)
        //{
        //    return;
        //}

        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, layerMask))
        {
            ground.PaintGrass(hitInfo.textureCoord);
            ground.GrassField.SetGrass(transform.position, playerNo);
        }

        timer = 0.0f;

    }
}
