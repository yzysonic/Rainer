using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSetting
{
    private static int numPlayer = 4;
    private static Dictionary<Joycon, int> joyconPlayerMap = new Dictionary<Joycon, int>();

    [RuntimeInitializeOnLoadMethod]
    public static void LoadDate()
    {
        var playerColorSetting = Resources.Load<PlayerColorSetting>("ScriptableObjects/DefaultPlayerColorSetting");
        PlayerColors[0] = playerColorSetting.player1;
        PlayerColors[1] = playerColorSetting.player2;
        PlayerColors[2] = playerColorSetting.player3;
        PlayerColors[3] = playerColorSetting.player4;
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