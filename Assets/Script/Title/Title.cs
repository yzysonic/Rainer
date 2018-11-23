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
    public CameraFallow cameraFallow;
    private Vector3 oldPlayerPos;

    private void Awake()
    {
        fadeInOut = FadeInOut.Instance;

        oldPlayerPos = titlePlayer.transform.position;
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
            BGMPlayer.Instance.Fade.Out();
        }

        if (titlePlayer.transform.position.x < -120.0f && !fadeInOut.enabled )
        {
            fadeInOut.FadeOut(() =>
            {
                titlePlayer.transform.position = oldPlayerPos;
                titlePlayer.DestroyCloud();
                titlePlayer.CreateCloud(true);
                Ground.Instance.ResetGrass();
                fadeInOut.FadeIn();
            });
        }

        if (Input.GetKeyDown(KeyCode.F) && !fadeInOut.enabled)
        {
            cameraFallow.target = null;
            fadeInOut.FadeOut(() => SceneManager.LoadScene("SettingScene"));

        }
    }
}
