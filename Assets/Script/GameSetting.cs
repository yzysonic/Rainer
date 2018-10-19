using System.Collections;
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
}
