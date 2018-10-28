using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

[RequireComponent(typeof(RawImage)), ExecuteInEditMode]
public class CircularGauge : MonoBehaviour
{
    #region Fields

    static readonly int MaxDivisoin = 4;

    [Range(0,100)]
    public float lerpSpeed = 10.0f;

    [SerializeField]
    private float radius = 0.5f;

    [SerializeField]
    private float width = 0.3f;

    [SerializeField]
    private List<Color> colors = new List<Color>(Enumerable.Repeat(Color.white, MaxDivisoin));

    [SerializeField]
    private float[] values = new float[MaxDivisoin];

    [SerializeField]
    private int division = MaxDivisoin;

    private Material material;
    private float[] ratios = new float[MaxDivisoin];

    #endregion

    #region Properties

    public float Radius
    {
        get
        {
            return radius;
        }
        set
        {
            material.SetFloat("_CircleRadius", value);
            radius = value;
        }
    }
    public float Width
    {
        get
        {
            return width;
        }
        set
        {
            material.SetFloat("_CircleWidth", value);
            width = value;
        }
    }
    public List<Color> Colors
    {
        get
        {
            return colors;
        }
        set
        {
            material.SetColorArray("_CircleColors", value);
            colors = value;
        }
    }
    public int Division
    {
        get
        {
            return division;
        }
        set
        {
            division = value;
            material.SetInt("_Division", division);
        }
    }
    public float[] Values
    {
        get
        {
            return values;
        }
        set
        {
            Array.Copy(value, values, 4);
        }
    }
    public float[] DisplayValues { get; set; } = new float[MaxDivisoin];

    #endregion

    #region Unity Methods

    private void OnValidate()
    {
        if (material == null)
            material = GetComponent<RawImage>().material;
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
        Init();
    }

    private void Update()
    {
        for(var i=0;i<division;i++)
        {
            DisplayValues[i] = Mathf.Lerp(DisplayValues[i], Values[i], Time.deltaTime * lerpSpeed);
            UpdateGauge();
        }
    }

    #endregion

    #region Methods

    public void UpdateGauge()
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
        Radius = radius;
        Width = width;
        Division = division;
        Colors = colors;

        UpdateGauge();
    }

    #endregion
}
