using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public class UserInformation
{
    public int coinCount;
    [field: SerializeField] private int levelIndex;
    //[field: SerializeField] private bool IsGetDailyBonus;
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private PlayerSettings settings;
    [field: SerializeField] private string lastLoginTime;
    public void Initialize()
    {
        coinCount = 0;
        levelIndex = 1;
        lastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        settings.Initialize();
    }

    public void SetUserID()
    {
        Id = SystemInfo.deviceUniqueIdentifier;
    }
    public int GetCoinCount => coinCount;
    public string GetUserId => Id;
    //public bool GetDailyBonusVariable => IsGetDailyBonus;
    //public void SetDailyBonusPossible(bool value) => IsGetDailyBonus = value;
    public void IncreaseCoinCount() => coinCount++;
    public void DecriseCoinCount() => coinCount--;
    public void IncreaseLevel() => levelIndex++;
    public int GetLevelIndex => levelIndex;
    public DateTime GetLastLoginDate => DateTime.ParseExact(lastLoginTime, "yyyy-MM-dd HH:mm:ss", null);
    public void SetCoin(int value) => coinCount = value;
    public void SetVolume(float value) => settings.SetVolume(value);
    public float GetVolume() => settings.MusicVolume;
    public bool IsVibrate() => settings.IsVibrate;
    public bool IsAdmob() => settings.isAdmob;
    public void SetVibrate(bool toggleValue) => settings.SetVibrate(toggleValue);
    public void SetAdmob(bool toggleValue) => settings.SetAdmob(toggleValue);
    public void UpdateLastLogin()
    {
        lastLoginTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        FirebaseManager.Instance.Save();
    }
}

