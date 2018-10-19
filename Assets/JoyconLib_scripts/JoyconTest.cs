using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconTest : MonoBehaviour
{
    public bool show_debug_info = true;

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joycon;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButton;

    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joycon = m_joycons[0];
    }

    private void Update()
    {
        m_pressedButton = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var button in m_buttons)
        {
            if (m_joycon.GetButton(button))
            {
                m_pressedButton = button;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_joycon.SetRumble(20, 40, 0.1f, 100);
        }

        if(m_joycon.GetButton(Joycon.Button.SHOULDER_2))
        {
            m_joycon.Recenter();
        }

        transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f) * m_joycon.GetVector();
    }

    private void OnGUI()
    {
        if (!show_debug_info)
            return;

        var style = GUI.skin.GetStyle("label");
        style.fontSize = 24;

        if (m_joycons == null || m_joycons.Count <= 0)
        {
            GUILayout.Label("Joy-Con が接続されていません");
            return;
        }

        GUILayout.BeginHorizontal(GUILayout.Width(960));

        var isLeft = m_joycon.isLeft;
        var name = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
        var key = "Z キー";
        var button = m_pressedButton;
        var stick = m_joycon.GetStick();
        var gyro = m_joycon.GetGyro();
        var accel = m_joycon.GetAccel();
        var orientation = m_joycon.GetVector();

        GUILayout.BeginVertical(GUILayout.Width(480));
        GUILayout.Label(name);
        GUILayout.Label(key + "：振動");
        GUILayout.Label("押されているボタン：" + button);
        GUILayout.Label(string.Format("スティック：({0}, {1})", stick[0], stick[1]));
        GUILayout.Label("ジャイロ：" + gyro);
        GUILayout.Label("加速度：" + accel);
        GUILayout.Label("傾き：" + orientation);
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }
}