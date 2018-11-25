using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class AudioFade : MonoBehaviour {

    public float fadeTime = 1.0f;
    [Range(0,1)] public float maxVolume = 1.0f;
    public bool stopAfterOut;
    public bool pauseAfterOut;

    private AnimationCurve curve;
    private Timer timer;
    private bool isFadeIn;
    private bool isFadeOut;

    public AudioSource AudioSource { get; set; }

    private void Reset()
    {
        enabled = false;
    }

    private void Awake()
    {
        timer = new Timer();
    }

    private void OnEnable()
    {
        if(!isFadeIn && !isFadeOut)
        {
            enabled = false;
            return;
        }

        if(AudioSource == null)
        {
            AudioSource = GetComponent<AudioSource>();
        }

        if (isFadeIn)
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.Play();
            }
            curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, maxVolume);
        }
        else
        {
            curve = AnimationCurve.Linear(0.0f, maxVolume, 1.0f, 0.0f);
        }

        timer.Reset(fadeTime);
    }
	
	// Update is called once per frame
	void Update ()
    {
        AudioSource.volume = curve.Evaluate(timer++);

        if (timer)
        {
            if (isFadeOut)
            {
                if (stopAfterOut)
                {
                    AudioSource.Stop();
                }
                else if(pauseAfterOut)
                {
                    AudioSource.Pause();
                }
            }

            enabled = false;
            isFadeIn = false;
            isFadeOut = false;
        }

    }

    public void In()
    {
        isFadeIn = true;
        enabled = true;
    }

    public void In(float fadeTime)
    {
        this.fadeTime = fadeTime;
        In();
    }

    public void Out()
    {
        isFadeOut = true;
        enabled = true;
    }

    public void Out(float fadeTime)
    {
        this.fadeTime = fadeTime;
        Out();
    }
}
