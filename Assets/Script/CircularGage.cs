using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(RawImage))]
public class CircularGage : MonoBehaviour {


    [Range(0.0f, 0.5f)]
    public float circleRadius = 0.5f;

    [Range(0.0f, 0.5f)]
    public float circleWidth = 0.15f;

    public List<Color> circleColors = new List<Color>(4);

    public float lerpSpeed = 10.0f;

    [SerializeField, Range(1,4)]
    private int division;

    private Material material;
    private float[] ratios;

    public int Division
    {
        get
        {
            return division;
        }
        set
        {
            division = value;
            Values = new float[division];
            DisplayValues = new float[division];
            ratios = new float[division];
            material.SetInt("_Division", division);
        }
    }
    public float[] Values { get; set; }
    public float[] DisplayValues { get; set; }

    protected void OnValidate()
    {
        circleWidth = Mathf.Min(circleWidth, circleRadius);

        if(circleRadius == circleWidth)
        {
            circleWidth = circleRadius;
        }

        if (material == null)
        {
            material = GetComponent<RawImage>().material;
        }
        material.SetFloat("_CircleRadius", circleRadius);
        material.SetFloat("_CircleWidth", circleWidth);
        material.SetColorArray("_CircleColors", circleColors);
        Division = division;
    }
    private void OnEnable()
    {
        GetComponent<RawImage>().enabled = enabled;
    }
    private void OnDisable()
    {
        GetComponent<RawImage>().enabled = enabled;
    }
    private void Awake()
    {
        material = GetComponent<RawImage>().material;
    }

    private void Update()
    {
        for(var i=0;i<division;i++)
        {
            DisplayValues[i] = Mathf.Lerp(DisplayValues[i], Values[i], Time.deltaTime * lerpSpeed);
            UpdateGage();
        }
    }

    public void UpdateGage()
    {
        var sum = DisplayValues.Sum();

        for (var i = 0; i < division; i++)
        {
            ratios[i] = DisplayValues[i] / sum;
        }

        material.SetFloatArray("_CircleRatios", ratios);
    }

    public void Init()
    {
        for (var i = 0; i < division; i++)
        {
            DisplayValues[i] = Values[i] = 1;
        }
        UpdateGage();
    }

}
