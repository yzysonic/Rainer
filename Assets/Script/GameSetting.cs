﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSetting
{
    private static int numPlayer = 4;
    private static Dictionary<Joycon, int> joyconPlayerMap = new Dictionary<Joycon, int>();

    [RuntimeInitializeOnLoadMethod]
    public static void LoadData()
    {
        PlayerColors = Resources.Load<PlayerColorSetting>("ScriptableObjects/DefaultPlayerColorSetting").colors;
        JoyconButton = Resources.Load<JoyconButtonSetting>("ScriptableObjects/DefaultJoyconButtonSetting");
    }

    public static int PlayerCount
    {
        get
        {
            return numPlayer;
        }
        set
        {
            numPlayer = Mathf.Clamp(value, 2, 4);
        }
    }

    public static Joycon[] PlayerJoycons { get; } = new Joycon[4];

    public static Color[] PlayerColors { get; private set; } = new Color[4];

    public static JoyconButtonSetting JoyconButton { get; private set; }

    public static void BindPlayer(this Joycon joycon, int player)
    {
        joycon.UnbindPlayer();

        if (player >= 0 && player < 4)
        {
            PlayerJoycons[player] = joycon;
            joyconPlayerMap.Add(joycon, player);
        }
    }

    public static void UnbindPlayer(this Joycon joycon)
    {
        var lastPlayer = -1;

        if (!joyconPlayerMap.TryGetValue(joycon, out lastPlayer) || joycon == null)
            return;

        PlayerJoycons[lastPlayer] = null;
        joyconPlayerMap.Remove(joycon);
    }

    public static int GetPlayerNo(this Joycon joycon)
    {
        int no;

        if (joyconPlayerMap.TryGetValue(joycon, out no) == false)
            return -1;
        else
            return no;
    }

    public enum Button
    {
        Join = Joycon.Button.DPAD_RIGHT,
        Cancel = Joycon.Button.DPAD_DOWN,
        Start = Joycon.Button.PLUS,
    }


    public static bool GetButton(this Joycon joycon, Button button)
    {
        return joycon.GetButton((Joycon.Button)button);
    }

    public static bool GetButtonUp(this Joycon joycon, Button button)
    {
        return joycon.GetButtonUp((Joycon.Button)button);
    }

    public static bool GetButtonDown(this Joycon joycon, Button button)
    {
        return joycon.GetButtonDown((Joycon.Button)button);
    }

}