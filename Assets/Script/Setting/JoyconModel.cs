using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyconModel : MonoBehaviour {

    public PlayerIcon playerIcon;
    private Renderer model;
    private float lpf = 10.0f;

    public bool show_debug_info = false;

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];
    
    private Joycon m_joycon;
    private Joycon.Button? m_pressedButton;

    // Use this for initialization
    void Start()
    {
        model = GetComponent<Renderer>();
        model.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(model.enabled != playerIcon.IsJoin)
        {
            model.enabled = playerIcon.IsJoin;
            m_joycon = playerIcon.Joycon;
        }

        if (m_joycon == null) return;

        m_pressedButton = null;

        foreach (var button in m_buttons)
        {
            if (m_joycon.GetButton(button))
            {
                m_pressedButton = button;
            }
        }

        //if (m_joycon.GetButton(Joycon.Button.SHOULDER_2))
        //{
        //    m_joycon.Recenter();
        //}

        // Joyconの向きのベクトルを計算
        var raw_vector = m_joycon.GetAccel();

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

        if (m_joycon == null)
        {
            GUILayout.Label("Joy-Con が未設定です");
            return;
        }

        GUILayout.BeginHorizontal(GUILayout.Width(960));

        var isLeft = m_joycon.isLeft;
        var name = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
        var button = m_pressedButton;
        var stick = m_joycon.GetStick();
        var gyro = m_joycon.GetGyro();
        var accel = m_joycon.GetAccel();
        var orientation = m_joycon.GetVector();

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
