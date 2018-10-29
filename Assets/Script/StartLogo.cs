using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartLogo : MonoBehaviour {

    //public AnimationClip clip;
    public Action callback;


    void Finished()
    {
        callback?.Invoke();
        gameObject.SetActive(false);
    }
}
