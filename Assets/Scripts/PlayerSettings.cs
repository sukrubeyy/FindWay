using System;
using UnityEngine;

[Serializable]
public struct PlayerSettings
{
    [Range(0,1)]
    public float MusicVolume;

    public bool IsVibrate;

    public bool isAdmob;


    public void SetVolume(float value) => MusicVolume = value;
}