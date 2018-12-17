using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;
using UnityEngine.Audio;

public class BGMPlayer : Singleton<BGMPlayer>
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private List<AudioSource> audioSources;

    public bool hasIntro;
    public float delay = 0.5f;

    public List<float> InitVolumes { get; private set; }
    public List<AudioFade> AudioFades { get; private set; }
    public List<AudioSource> AudioSources { get; private set; }
    public AudioFade Fade
    {
        get
        {
            return AudioFades?.Find(f => f.AudioSource.isPlaying) ?? AudioFades[0];
        }
    }

    protected override void Awake()
    {
        base.Awake();

        InitVolumes = new List<float>();
        AudioFades = new List<AudioFade>();
        AudioSources = audioSources;

        if (AudioSources == null)
        {
            AudioSources = new List<AudioSource>();
        }
        else
        {
            foreach (var source in AudioSources)
            {
                InitVolumes.Add(source.volume);
                AddFade(source);
            }
        }


        foreach (var clip in audioClips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.playOnAwake = false;
            source.outputAudioMixerGroup = audioMixerGroup;
            AudioSources.Add(source);
            InitVolumes.Add(source.volume);
            AddFade(source);
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

    private void AddFade(AudioSource source)
    {
        var fade = gameObject.AddComponent<AudioFade>();
        fade.AudioSource = source;
        fade.maxVolume = source.volume;
        AudioFades.Add(fade);
    }
}
