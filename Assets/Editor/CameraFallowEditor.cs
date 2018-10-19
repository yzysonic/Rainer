using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraFallow))]
public class CameraFallowEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (CameraFallow)target;

        EditorGUI.BeginChangeCheck();

        CameraFallow.Angle = EditorGUILayout.Slider("Angle", CameraFallow.Angle, 0.0f, 0.5f*Mathf.PI);
        CameraFallow.Distance = EditorGUILayout.FloatField("Distance", CameraFallow.Distance);

        if (EditorGUI.EndChangeCheck())
        {
            script.UpdataPosition();
        }
    }

    
}