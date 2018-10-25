using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    [Range(0, 200)]
    public int initTime = 60;
    private Text text;

    public float RemainingTime { get; private set; }
    public bool TimesUp
    {
        get
        {
            return RemainingTime <= 0.0f;
        }
    }

    private void OnValidate()
    {
        GetComponent<Text>().text = $"Time: {(int)initTime}";
    }

    // Use this for initialization
    void Start () {
        RemainingTime = initTime;
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        RemainingTime -= Time.deltaTime;

        text.text = $"Time {(int)RemainingTime}";
	}
}
