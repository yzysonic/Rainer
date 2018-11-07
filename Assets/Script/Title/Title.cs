using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Title : MonoBehaviour
{

    public Vector3 tSize;
    public float tRotY;
    private FadeInOut fadeInOut;

    private void Awake()
    {
        tSize = new Vector3(0.1f, 0.1f, 0.1f);
        tRotY = 0f;
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
    }
}
