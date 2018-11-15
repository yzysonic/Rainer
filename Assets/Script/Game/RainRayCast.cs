using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainRayCast : MonoBehaviour {

    public float delay = 0.5f;

    private RaycastHit hitInfo;
    private Ground ground;
    private float timer;
    private int layerMask;
    private int playerNo;
    private Queue<Vector2> moveHistory;

    // Use this for initialization
    void Start ()
    {
        ground = Ground.Instance;
        layerMask = LayerMask.GetMask("Ground");
        playerNo = transform.parent.GetComponent<Cloud>().target.GetComponent<PlayerController>().PlayerNo;
        moveHistory = new Queue<Vector2>();

        for (var i = 0; i < delay / Time.fixedDeltaTime; i++)
        {
            EnqueuePos();
        }
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        if (EnqueuePos())
        {
            var uv = moveHistory.Dequeue();
            ground.GrowGrass(uv, playerNo);
        }

    }

    private bool EnqueuePos()
    {
        bool hit;

        if (hit = Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, layerMask))
        {
            moveHistory.Enqueue(hitInfo.textureCoord);
        }

        return hit;
    }
}
