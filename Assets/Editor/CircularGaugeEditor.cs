using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System.Linq;



[CustomEditor(typeof(CircularGauge), true)]
public class CircularGaugeEditor : Editor
{

    private List<Color> colors = new List<Color>();
    protected void OnEnable()
    {
        Debug.Log("enable");
        var gauge = target as CircularGauge;
        gauge.Init();
    }

    public override void OnInspectorGUI()
    {
        var gauge       = target as CircularGauge;
        var radius      = gauge.Radius;
        var width       = gauge.Width;
        var division    = gauge.Division;
        var colors      = gauge.Colors;

        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("lerpSpeed"));

        EditorGUI.BeginChangeCheck();

        radius      = EditorGUILayout.Slider("Radius", radius, 0.0f, 0.5f);
        width       = EditorGUILayout.Slider("Width", width, 0.0f, 0.5f);
        division    = EditorGUILayout.IntSlider("Division", division, 1, 4);

        for (var i = 0; i < gauge.Division; i++)
        {
            colors[i] = EditorGUILayout.ColorField($"Color{i + 1}", colors[i]);
        }

        if(EditorGUI.EndChangeCheck())
        {
            gauge.Radius    = radius;
            gauge.Width     = width;
            gauge.Division  = division;
            gauge.Colors    = colors;
        }

        serializedObject.ApplyModifiedProperties();

    }

}
