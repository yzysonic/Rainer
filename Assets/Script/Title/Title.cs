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
        AudioMixerFade.Instance.Set(1.0f);
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
            AudioMixerFade.Instance.Out();
            GetComponent<AudioSource>().Play();
            startAnimation.Play("TitleStart");
            JoyconManager.Instance.Destroy();
            fadeInOut.FadeOut(3.0f, () =>
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            JoyconManager.Instance.Destroy();
        }


    }
}
