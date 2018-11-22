using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Test : MonoBehaviour
{

    private AudioFade audioFade;

    private void Start()
    {
        audioFade = GetComponent<AudioFade>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (GetComponent<AudioSource>().volume == 0.0f)
            {
                audioFade.In();
            }
            else
            {
                audioFade.Out();
            }
        }
    }


}