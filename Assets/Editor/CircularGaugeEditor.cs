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

    private float lastWidth;

    public override void OnInspectorGUI()
    {
        var gauge       = target as CircularGauge;
        var radius      = gauge.Radius;
        var width       = gauge.Width;
        var division    = gauge.Division;
        var colors      = gauge.Colors;
        var values      = gauge.Values;

        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("lerpSpeed"));

        EditorGUI.BeginChangeCheck();

        radius      = EditorGUILayout.Slider("Radius", radius, 0.0f, 0.5f);
        width       = Mathf.Min(EditorGUILayout.Slider("Width", width, 0.0f, 0.5f), radius);
        division    = EditorGUILayout.IntSlider("Division", division, 1, 4);

        for (var i = 0; i < gauge.Division; i++)
        {
            colors[i] = EditorGUILayout.ColorField($"Color{i + 1}", colors[i]);
        }

        for(var i=0;i<gauge.Division;i++)
        {
            values[i] = Mathf.Max(EditorGUILayout.FloatField($"Value{i + 1}", values[i]), 0.0f);
        }

        if(EditorGUI.EndChangeCheck())
        {

            gauge.Radius    = radius;
            gauge.Width     = width;
            gauge.Division  = division;
            gauge.Colors    = colors;
            gauge.Values    = values;

            lastWidth       = width;
            //gauge.UpdateGauge();
        }

        serializedObject.ApplyModifiedProperties();

    }

}
