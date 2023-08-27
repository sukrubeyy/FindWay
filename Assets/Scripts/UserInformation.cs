using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public class UserInformation
{
    public int coinCount;
    [field: SerializeField] private int levelIndex;
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private PlayerSettings settings;
    [field: SerializeField] private string lastLoginTime;
    [field: SerializeField] private CustomizationSettings _customizationSettings;

    public CustomizationSettings GetCustomizationSettings => _customizationSettings;
    public void Initialize()
    {
        coinCount = 0;
        levelIndex = 1;
        lastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        settings.Initialize();
        _customizationSettings.Initialize();
    }

    public void SetUserID()
    {
        Id = SystemInfo.deviceUniqueIdentifier;
    }
    public int GetCoinCount => coinCount;
    public string GetUserId => Id;
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

    public void SetEyesColor(Color32 value)
    {
        _customizationSettings.SetEyesColor(value);  
    }

    public Color32 GetEyesColor => _customizationSettings.EyesColor;
    public Color32 GetBodyColor => _customizationSettings.BodyColor;
    public Color32 GetArmsColor => _customizationSettings.ArmsColor;
    public void SetBodyColor(Color32 value)
    {
        _customizationSettings.SetBodyColor(value);
    }

    public void SetArmsColor(Color32 value)
    {
        _customizationSettings.SetArmsColor(value);
    }
}