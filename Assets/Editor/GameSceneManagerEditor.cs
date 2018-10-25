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

        GameSetting.NumPlayer = EditorGUILayout.IntSlider("プレイ人数", GameSetting.NumPlayer, 2, 4);

        if (EditorGUI.EndChangeCheck())
        {
            gcm.SetPlayer();
        }
    }
}
