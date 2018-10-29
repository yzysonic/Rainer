using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraFallow))]
public class CameraFallowEditor : Editor
{

    private SerializedProperty angle;
    private SerializedProperty distance;

    private void OnEnable()
    {
        angle = serializedObject.FindProperty("angle");
        distance = serializedObject.FindProperty("distance");
    }

    public override void OnInspectorGUI()
    {

        //serializedObject.Update();

        var script = (CameraFallow)target;

        EditorGUI.BeginChangeCheck();

        DrawDefaultInspector();
        //EditorGUILayout.PropertyField(angle);
        //EditorGUILayout.PropertyField(distance);
        //CameraFallow.Angle = EditorGUILayout.Slider("Angle", CameraFallow.Angle, 0.0f, 0.5f * Mathf.PI);
        //CameraFallow.Distance = EditorGUILayout.FloatField("Distance", CameraFallow.Distance);

        if (EditorGUI.EndChangeCheck())
        {
            script.UpdatePosition();
        }

        //serializedObject.ApplyModifiedProperties();
    }

}