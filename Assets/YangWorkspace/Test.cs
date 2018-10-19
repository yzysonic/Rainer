using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class Test : MonoBehaviour
{

    private System.Action callback;

    [SerializeField]
    private GameObject player;

    private void Start()
    {
        Fade(() =>
        {
            SceneManager.LoadScene("SettingScene");
        });
        Fade();
    }

    private void Update()
    {

    }

    void Fade(System.Action callback = null)
    {
        this.callback = callback;
    }
}