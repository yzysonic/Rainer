using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartLogo : MonoBehaviour {

    //public AnimationClip clip;
    public Action callback;
    public List<AudioClip> audioClips;


    void Finished()
    {
        callback?.Invoke();
        gameObject.SetActive(false);
    }

    void PlayReadyAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
    }

    void PlayStartAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(audioClips[1]);
    }
}
