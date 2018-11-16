using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using RainerLib;

public class Title : MonoBehaviour
{
    private FadeInOut fadeInOut;
    public PlayerControl titlePlayer;
    public CameraFallow CameraFallow;
    public CameraLookAt CameraLookAt;


    private void Awake()
    {
        fadeInOut = FadeInOut.Instance;

    }

    // Use this for initialization
    void Start ()
    {
        fadeInOut.FadeIn();

    }

	// Update is called once per frame
	void Update ()
    {

        if (Input.GetButtonDown("Submit")  && !fadeInOut.enabled)
        {
            fadeInOut.FadeOut(() => SceneManager.LoadScene("SettingScene"));
        }

        //if (titlePlayer.position.x < 50.0f)
        //{
        //    fadeInOut.FadeOut(() => SceneManager.LoadScene("SettingScene"));
        //}

        if (Input.GetKeyDown(KeyCode.F))
        {
            CameraFallow.target = null;
            //fadeInOut.FadeOut(() => SceneManager.LoadScene("TitleScene"));

        }
    }
}
