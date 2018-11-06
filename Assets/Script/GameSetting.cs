using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSetting
{
    private static int numPlayer = 4;
    private static Dictionary<Joycon, int> joyconPlayerMap;

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

    public static void BindPlayer(this Joycon joycon, int player)
    {
        if (player >= 0 && player < 4)
        {
            PlayerJoycons[player] = joycon;
            joyconPlayerMap[joycon] = player;
        }
    }

    public static int GetPlayerNo(this Joycon joycon)
    {
        int no;

        if (joyconPlayerMap.TryGetValue(joycon, out no) == false)
            return -1;
        else
            return no;
    }
}
