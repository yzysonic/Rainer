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
    public Animation startAnimation;
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

        if ((Input.GetButtonDown("Submit") || JoyconManager.GetButtonDown(GameSetting.JoyconButton.Start)) && !fadeInOut.enabled)
        {
            cameraFallow.target = null;
            BGMPlayer.Instance.AudioFades[0].Out();
            BGMPlayer.Instance.AudioFades[1].Out();
            startAnimation.Play("TitleStart");
            fadeInOut.FadeOut(() =>
            {
                BGMPlayer.Instance.Destroy();
                SceneManager.LoadScene("SettingScene");
            });
        }

        if (titlePlayer.transform.position.x < -145.0f && !fadeInOut.enabled )
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


    }
}
