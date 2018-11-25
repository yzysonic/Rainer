using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = RainerLib.Tree;

public class Seed : MonoBehaviour {

    public Tree Tree { get; private set; }

	// Use this for initialization
	void Awake () {
        Tree = GetComponentInChildren<Tree>();
    }

    private void OnDisable()
    {
        transform.Find("Model").gameObject.SetActive(false);
        transform.Find("Icon").gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "RainArea")
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
