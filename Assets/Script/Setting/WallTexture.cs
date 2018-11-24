using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTexture : MonoBehaviour {
    private Material material;
    private float alpha;
    private int propertyId;

    // Use this for initialization
    void Start () {
        material = GetComponent<Renderer>().material;
        propertyId = Shader.PropertyToID("_Alpha");
	}
	
	// Update is called once per frame
	void Update () {
        alpha = Vector3.Dot(transform.up, Vector3.back);
        material.SetFloat(propertyId, (1 + alpha * 3) * 0.25f);
	}
}
