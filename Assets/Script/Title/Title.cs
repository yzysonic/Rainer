using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    public Vector3 tSize;
    public float tRotY;
    public FadeInOut fadeInOut;

    private void Awake()
    {
        tSize = new Vector3(0.1f, 0.1f, 0.1f);
        tRotY = 0f;

    }

    // Use this for initialization
    void Start ()
    {
        fadeInOut.Alpha = 1.0f;
        fadeInOut.fadeIn();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fadeInOut.fadeOut(() => SceneManager.LoadScene("SettingScene"));
        }
    }
}
