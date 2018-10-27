using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System.Linq;



[CustomEditor(typeof(CircularGage), true)]
[CanEditMultipleObjects]
public class CircularGageEditor : Editor
{
    private CircularGage gage;

    protected void OnEnable()
    {
        gage = target as CircularGage;
        gage.Init();
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        var material = gage.GetComponent<RawImage>().material;

    }

}
