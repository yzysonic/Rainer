using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartLogo : MonoBehaviour {

    public AnimationClip clip;
    public Action callback;

    private void Awake()
    {
        var animation = gameObject.AddComponent<Animation>();
        animation.AddClip(clip, clip.name);
        animation.clip = clip;
        var a = new AnimationEvent();
        a.functionName = "Finished";
        a.time = clip.length;
        clip.AddEvent(a);
        animation.Play();
    }


    void Finished()
    {
        callback?.Invoke();
        gameObject.SetActive(false);
    }
}
