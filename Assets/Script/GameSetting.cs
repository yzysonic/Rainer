using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSetting
{
    private static int numPlayer = 4;
    private static Dictionary<Joycon, int> joyconPlayerMap = new Dictionary<Joycon, int>();

    /// <summary>
    /// 設定データをロードして保存する
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadAndSetData()
    {
        // 色設定
        DefaultPlayerColors = LoadScriptableObject<PlayerColorSetting>()?.colors;
        System.Array.Copy(DefaultPlayerColors, PlayerColors, DefaultPlayerColors.Length);
        PlayerColorIndex = LoadScriptableObject<PlayerColorIndex>()?.values ?? new int[4] { 0, 1, 2, 3 };

        // Joycon設定
        JoyconButton = LoadScriptableObject<JoyconButtonSetting>();
        JoyconSerialNumbers = LoadScriptableObject<JoyconSerialNumberSetting>()?.serialNumbers;
    }

    /// <summary>
    /// 設定データをロードして返す
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>設定データのオブジェクト</returns>
    public static T LoadScriptableObject<T>() where T : UnityEngine.Object
    {
        return 
            Resources.Load<T>($"ScriptableObjects/{typeof(T).Name}") ?? 
            Resources.Load<T>($"ScriptableObjects/Default{typeof(T).Name}");
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
    public static Color[] DefaultPlayerColors { get; private set; } = new Color[4];

    public static Color[] PlayerColors { get; set; } = new Color[4];

    public static int[] PlayerColorIndex { get; set; }


    public static JoyconButtonSetting JoyconButton { get; private set; }

    public static List<string> JoyconSerialNumbers { get; private set; }

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
    public static bool GetButton(this Joycon joycon, Joycon.Button button)
    {
        return joycon.GetButton(button);
    }

    public static bool GetButtonUp(this Joycon joycon, Joycon.Button button)
    {
        return joycon.GetButtonUp(button);
    }

    public static bool GetButtonDown(this Joycon joycon, Joycon.Button button)
    {
        return joycon.GetButtonDown(button);
    }

}