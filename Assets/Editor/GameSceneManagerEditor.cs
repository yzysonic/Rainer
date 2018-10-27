using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameSceneManager))]
public class GameSceneManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var gcm = GameSceneManager.Instance;

        EditorGUI.BeginChangeCheck();

        GameSetting.PlayerCount = EditorGUILayout.IntSlider("PlayerCount", GameSetting.PlayerCount, 2, 4);

        if (EditorGUI.EndChangeCheck())
        {
            gcm.SetPlayer();
        }
    }
}
