using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyconModel : MonoBehaviour {

    public PlayerIcon playerIcon;
    private Renderer model;

    // Use this for initialization
    void Start()
    {
        model = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        model.enabled = playerIcon.IsJoin;
    }
}
