using System;
using UnityEngine;

[Serializable]
public class UserInformation
{
    public int coinCount;
    [field: SerializeField] private int levelIndex;
    [field: SerializeField] private bool IsGetDailyBonus;
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private PlayerSettings settings;
    public void Initialize()
    {
        Id = SystemInfo.deviceUniqueIdentifier;
    }   
    public int GetCoinCount => coinCount;
    public string GetUserId => Id;
    public bool GetDailyBonusVariable => IsGetDailyBonus;
    public void SetDailyBonusPossible(bool value) => IsGetDailyBonus = value;
    public void IncreaseCoinCount() => coinCount++;
    public void DecriseCoinCount() => coinCount--;
    public void IncreaseLevel() => levelIndex++;
    public void SetCoin(int value) => coinCount = value;
    public void SetVolume(float value) => settings.SetVolume(value);
    public float GetVolume() => settings.MusicVolume;
}

