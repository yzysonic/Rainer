using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyconModel : MonoBehaviour {

    private float lpf = 10.0f;

    public bool show_debug_info = false;

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];
    
    private Joycon.Button? m_pressedButton;
    public Joycon Joycon { get; set; }

    // Update is called once per frame
    void Update()
    {

        if (Joycon == null) return;

        m_pressedButton = null;

        foreach (var button in m_buttons)
        {
            if (Joycon.GetButton(button))
            {
                m_pressedButton = button;
            }
        }

        // Joyconの向きのベクトルを計算
        var raw_vector = Joycon.GetAccel();

        var targetRotation = Quaternion.Euler(raw_vector.z * 60.0f, 180.0f, -raw_vector.y * 60.0f);

        // モデル回転に適用
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, lpf * Time.deltaTime);
        //transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f) * m_joycon.GetVector();

    }

    private void OnGUI()
    {
        if (!show_debug_info)
            return;

        var style = GUI.skin.GetStyle("label");
        style.fontSize = 24;

        if (Joycon == null)
        {
            GUILayout.Label("Joy-Con が未設定です");
            return;
        }

        GUILayout.BeginHorizontal(GUILayout.Width(960));

        var isLeft = Joycon.isLeft;
        var name = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
        var button = m_pressedButton;
        var stick = Joycon.GetStick();
        var gyro = Joycon.GetGyro();
        var accel = Joycon.GetAccel();
        var orientation = Joycon.GetVector();

        GUILayout.BeginVertical(GUILayout.Width(480));
        GUILayout.Label(name);
        GUILayout.Label("押されているボタン：" + button);
        GUILayout.Label(string.Format("スティック：({0}, {1})", stick[0], stick[1]));
        GUILayout.Label("ジャイロ：" + gyro);
        GUILayout.Label("加速度：" + accel);
        GUILayout.Label("傾き：" + orientation);
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }
}
