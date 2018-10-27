using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

    [Range(0, 200)]
    public int initTime = 60;

    private Text text;
    private CircularGauge gauge;
    private float remainingTime;

    public float RemainingTime
    {
        get
        {
            return remainingTime;
        }
        set
        {
            var displayTime = Mathf.RoundToInt(value);
            text.text = displayTime.ToString();
            gauge.Values[0] = initTime - value;
            gauge.Values[1] = value;
            remainingTime = value;
        }
    }
    public bool TimesUp
    {
        get
        {
            return RemainingTime <= 0.0f;
        }
    }

    private void OnValidate()
    {
        GetComponentInChildren<Text>().text = initTime.ToString();
    }

    // Use this for initialization
    void Start () {
        text = GetComponentInChildren<Text>();
        gauge = GetComponentInChildren<CircularGauge>();
        RemainingTime = initTime;
    }
	
	// Update is called once per frame
	void Update () {
        RemainingTime -= Time.deltaTime;
	}

}
