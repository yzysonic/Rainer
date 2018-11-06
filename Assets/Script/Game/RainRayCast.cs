using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainRayCast : MonoBehaviour {

    private RaycastHit hitInfo;
    private GrassSpawn grassSpawn;
    private Ground ground;
    private float timer;
    private int layerMask;

    // Use this for initialization
    void Start () {
        //grassSpawn = GameObject.Find("Ground").GetComponent<GrassSpawn>();
        ground = GameObject.Find("Ground").GetComponent<Ground>();
        timer = 0.0f;
        layerMask = LayerMask.GetMask("Ground");
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
            //grassSpawn.Spawn(hitInfo);
            ground.PaintGrass(hitInfo.textureCoord);
        }

        timer = 0.0f;

    }
}
