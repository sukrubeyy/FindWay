using System;
using UnityEngine;

[Serializable]
public struct UserData
{
    [field: SerializeField] private int coinCount;
    [field: SerializeField] private int levelIndex;
    private DateTime lastLoginTime;

    public PlayerSettings playerSettings;
    public DateTime GetLastLoginTime => lastLoginTime;
    public int GetCoinCount => coinCount;
    public int GetLevelIndex => levelIndex;
    public void IncreaseCoinCount() => coinCount++;
    public void DecriseCoinCount() => coinCount--;
    
}