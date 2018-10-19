using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainRayCast : MonoBehaviour {

    private RaycastHit hitInfo;
    private GrassSpawn grassSpawn;
    private float timer;

    // Use this for initialization
    void Start () {
        grassSpawn = GameObject.Find("Ground").GetComponent<GrassSpawn>();
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        timer += Time.deltaTime;
        //if (timer <= 0.6f)
        //{
        //    return;
        //}

        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
        {
            grassSpawn.Spawn(hitInfo);
        }

        timer = 0.0f;

    }
}
