using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(GameSceneManager))]
public class GameSceneManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        var gcm = GameSceneManager.Instance;

        if (GUILayout.Button("SetPlayerAndCamera"))
        {
            gcm.SetPlayerCount();
            gcm.SetPlayer();
            gcm.SetCamera();
        }

        if (GUILayout.Button("SetPlayerColor"))
        {
            gcm.SetPlayerColor();
        }

        if (GUILayout.Button("SetCamerasRect"))
        {
            gcm.SetCameraRect();
        }
    }

}
