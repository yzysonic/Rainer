using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;
using UnityEngine.SceneManagement;

public class DebugManager : Singleton<DebugManager>
{

#if UNITY_EDITOR

    [RuntimeInitializeOnLoadMethod]
    static void Create()
    {

        if (Application.isEditor)
        {
            var instance = Instance;
        }
    }

#endif

    BGMPlayer BGMPlayer
    {
        get
        {
            return BGMPlayer.IsCreated ? BGMPlayer.Instance : null;
        }
    }
    FadeInOut FadeInOut
    {
        get
        {
            return FadeInOut.IsCreated ? FadeInOut.Instance : null;
        }
    }
    bool pause;
    float targetTimeScale;
    GUIStyle style;
    GUIStyleState styleState;

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        styleState = new GUIStyleState();
        styleState.textColor = Color.white;
        style.normal = styleState;
        targetTimeScale = 1.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, 0.2f);
        if (BGMPlayer?.AudioFades?.Count > 0)
            BGMPlayer.Fade.AudioSource.pitch = Time.timeScale;


        if (FadeInOut?.enabled ?? false)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.F1))
        {
            LoadScene("TitleScene");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadScene("SettingScene");
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            LoadScene("GameScene");
        }

        if (Input.GetKeyDown(KeyCode.Pause) || JoyconManager.GetButtonDown(Joycon.Button.HOME))
        {
            pause = !pause;
            targetTimeScale = pause ? 0.0f : 1.0f;
        }

        if (GameTimer.IsCreated)
        {
            if (JoyconManager.GetButtonDown(Joycon.Button.PLUS))
            {
                GameTimer.Instance.RemainingTime = 31.0f;
            }

            //if (JoyconManager.GetButton(Joycon.Button.SHOULDER_2))
            //{
            //    targetTimeScale = 3.0f* Mathf.Max(JoyconManager.Instance.j[0].GetStick()[1], 0.0f);
            //}
            //else if (JoyconManager.GetButtonUp(Joycon.Button.SHOULDER_2))
            //{
            //    targetTimeScale = 1.0f;
            //}
        }


    }

    private void OnGUI()
    {
        if (!pause)
            return;

        GUI.Label(new Rect(20, 20, 100, 50),"PAUSE", style);

    }

    void LoadScene(string sceneName)
    {
        pause = false;
        BGMPlayer?.Fade.Out(0.3f);
        FadeInOut.Instance.FadeOut(0.3f, () =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}
