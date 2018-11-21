using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainerCount : MonoBehaviour
{
    [Range(1,3)]
    public int digit = 1;

    private Text text;

    [SerializeField]
    private int value;

    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            text.text = GetValueString();
        }
    }

    private void OnValidate()
    {
        var text = GetComponent<Text>();
        text.text = GetValueString();
    }

    // Use this for initialization
    void Awake () {
        text = GetComponent<Text>();
	}

    string GetValueString()
    {
        return $"x{value.ToString($"d{digit}")}";
    }

}
