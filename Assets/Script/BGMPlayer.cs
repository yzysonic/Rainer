using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class BGMPlayer : Singleton<BGMPlayer> {

    public List<AudioClip> audioClips;
    public bool hasIntro;
    public float delay = 0.5f;

    public List<AudioSource> AudioSources { get; private set; } = new List<AudioSource>();
    public List<AudioFade> AudioFades { get; private set; } = new List<AudioFade>();
    public AudioFade Fade
    {
        get
        {
            return AudioFades.Find(f => f.AudioSource.isPlaying) ?? AudioFades[0];
        }
    }

    protected override void Awake()
    {
        base.Awake();

        foreach(var clip in audioClips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.playOnAwake = false;
            AudioSources.Add(source);

            var fade = gameObject.AddComponent<AudioFade>();
            fade.AudioSource = source;
            AudioFades.Add(fade);
        }

        if (hasIntro && AudioSources.Count >= 2)
        {
            AudioSources[1].loop = true;
            AudioSources[0].PlayScheduled(AudioSettings.dspTime + delay);
            AudioSources[1].PlayScheduled(AudioSettings.dspTime + AudioSources[0].clip.length + delay);
        }
        else
        {
            AudioSources[0].Play();
        }
    }
}
