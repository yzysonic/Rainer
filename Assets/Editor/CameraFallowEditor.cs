using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraFallow))]
public class CameraFallowEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var script = (CameraFallow)target;

        EditorGUI.BeginChangeCheck();

        DrawDefaultInspector();

        if (EditorGUI.EndChangeCheck())
        {
            script.UpdatePosition();
        }

    }

}