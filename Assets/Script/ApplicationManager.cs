using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ApplicationManager : Singleton<ApplicationManager>
{

    [RuntimeInitializeOnLoadMethod]
    static void Create()
    {
        var instance = Instance;
    }

    // Use this for initialization
    protected override void Awake () {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

#if !UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("TitleScene");
        }

#endif

    }
}
