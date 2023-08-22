using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : Singleton<AudioManager>
{
    public List<AudioStruct> audios;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ExecuteClip(AudioClipType type)
    {
        var clipData = audios.Find(e => e.clipType == type);
        audioSource.clip = clipData.clips[0];
        audioSource.playOnAwake = true;
        audioSource.Play();
    }
}

[Serializable]
public struct AudioStruct
{
    public AudioClipType clipType;
    public List<AudioClip> clips;
}

public enum AudioClipType
{
    Jump1,
    Jump2,
    Dash
}