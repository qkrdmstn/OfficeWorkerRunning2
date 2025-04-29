using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    SFX,
    BGM
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float sfxVolume = 1.0f;
    [Range(0f, 1f)] public float bgmVolume = 1.0f;

    [Header("Audio Clips")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(gameObject);

        for(int i=0; i<bgmClips.Length; i++)
            bgmDict.Add(bgmClips[i].name, bgmClips[i]);
        for(int i=0; i<sfxClips.Length; i++)
            sfxDict.Add(sfxClips[i].name, sfxClips[i]);
    }

    public void Play(string clipName, SoundType type = SoundType.SFX, bool loop = false)
    {
        switch (type)
        {
            case SoundType.BGM:
                if (bgmDict.ContainsKey(clipName))
                {
                    bgmSource.clip = bgmDict[clipName];
                    bgmSource.loop = loop;
                    bgmSource.volume = bgmVolume;
                    bgmSource.Play();
                }
                break;

            case SoundType.SFX:
                if (sfxDict.ContainsKey(clipName))
                {
                    sfxSource.PlayOneShot(sfxDict[clipName], sfxVolume);
                }
                break;
        }
    }

    public void Stop(SoundType type)
    {
        switch (type)
        {
            case SoundType.BGM:
                bgmSource.Stop();
                break;
            case SoundType.SFX:
                sfxSource.Stop();
                break;
        }
    }

    public void SetVolume(SoundType type, float volume)
    {
        switch (type)
        {
            case SoundType.BGM:
                bgmVolume = volume;
                bgmSource.volume = volume;
                break;
            case SoundType.SFX:
                sfxVolume = volume;
                sfxSource.volume = volume;
                break;
        }
    }
}
