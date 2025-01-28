using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class SoundManager : Singleton<SoundManager>
{
    static AudioSource audioSource;
    static SoundData soundData;
    public static AudioClip loopingClip;
    float volume = 0.2f;
    static Tween audioStop;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
        soundData = GetComponent<SoundData>();
    }

    public static void Play(string clipname)
    {
        AudioClip clip = soundData.GetRandomClip(clipname);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public static void PlayforDuration(string clipname, float duration)
    {
        AudioClip clip = soundData.GetRandomClip(clipname);

        if (!clip)
        {
            return;
        }

        if (audioSource.isPlaying && audioStop != null && audioStop.IsActive())
        {
            audioStop.Kill();
        }

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
        audioStop = DOVirtual.DelayedCall(duration, () => audioSource.Stop());
    }

    public static void PlayLoop(string clipname)
    {
        AudioClip clip = soundData.GetRandomClip(clipname);
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
            loopingClip = clip;
        }
    }

    public static void StopLoop()
    { 
        if (audioSource.clip == loopingClip)
        {
            audioSource.Stop();
        }
    }
}
