using System;
using UnityEngine;

[Serializable]
public struct PlayerSettings
{
    [Range(0,1)]
    public float MusicVolume;
    public bool IsVibrate;
    public bool isAdmob;

    public void Initialize()
    {
        MusicVolume = 1f;
        IsVibrate = true;
        isAdmob = true;
    }
    public void SetVolume(float value) => MusicVolume = value;
    public void SetVibrate(bool toggleValue) => IsVibrate = toggleValue;
    public void SetAdmob(bool toggleValue) => isAdmob = toggleValue;
}