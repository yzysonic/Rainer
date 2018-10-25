﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSetting
{
    private static int numPlayer = 2;

    public static int NumPlayer
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

    public static void BindPlayer(this Joycon joycon, int player)
    {
        if (player < 0 || player > 4)
            return;

        PlayerJoycons[player] = joycon;
    }
}