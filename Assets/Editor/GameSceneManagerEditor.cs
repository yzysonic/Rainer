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

        EditorGUI.BeginChangeCheck();

        GameSetting.NumPlayer = EditorGUILayout.IntSlider("プレイ人数", GameSetting.NumPlayer, 2, 4);

        //if (GUILayout.Button("適用"))
        //{
        //    GameSceneManager.Instance.SetPlayerAndCamera();
        //}

        if (EditorGUI.EndChangeCheck())
        {
            GameSceneManager.Instance.SetPlayerAndCamera();
        }
    }
}
