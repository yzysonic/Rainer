using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using RainerLib;

public class AudioMixerFade : Singleton<AudioMixerFade>
{

    public AudioMixer audioMixer;
    public List<string> fadeGroups;
    [Range(0, 1)] public float maxVolume = 1.0f;
    [Range(0, 1)] public float minVolume = 0.0f;
    private AnimationCurve curve;
    private Timer timer;
    private float fadeTime;
    private bool isFadeIn;
    private bool isFadeOut;

    private void Reset()
    {
        enabled = false;
    }

    protected override void Awake()
    {
        base.Awake();
        timer = new Timer();
    }

    private void OnEnable()
    {
        if (!isFadeIn && !isFadeOut)
        {
            enabled = false;
            return;
        }
        if (isFadeIn)
        {
            curve = AnimationCurve.Linear(0.0f, minVolume, 1.0f, maxVolume);
        }
        else
        {
            curve = AnimationCurve.Linear(0.0f, maxVolume, 1.0f, minVolume);
        }

        timer.Reset(fadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        var volume = Mathf.Lerp(-80.0f, 0.0f, curve.Evaluate(Mathf.Min(timer.Progress, 1.0f)));
        Set(volume);

        if (timer)
        {
            enabled = false;
            isFadeIn = false;
            isFadeOut = false;
        }

    }

    public void In(float fadeTime = 1.0f)
    {
        this.fadeTime = fadeTime;
        isFadeIn = true;
        enabled = true;
    }

    public void Out(float fadeTime = 1.0f)
    {
        this.fadeTime = fadeTime;
        isFadeOut = true;
        enabled = true;
    }

    public void Set(float volume)
    {
        foreach (var group in fadeGroups)
        {
            audioMixer.SetFloat(group, volume);
        }
    }
}


