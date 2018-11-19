using UnityEngine;
using UnityEditor;
using System.Collections;



[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }



}
