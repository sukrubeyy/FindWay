using System;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public UserInformation userInformation;
    public bool isDelete;
    private void Start()
    {
        userInformation.SetUserID();
    }

    private void Update()
    {
        if (isDelete)
        {
            isDelete = false;
            FirebaseManager.Instance.DeleteData();
        }
    }

    public void InstanceOnDailyBonusEvent(DateTime lastLogin)
    {
        TimeSpan timeDifference = DateTime.Now - lastLogin;
        if (timeDifference.TotalDays >= 1)
        {
            MenuManager.Instance.isDailyBonus = true;
            userInformation.UpdateLastLogin();
        }
    }

    public void SetCustomizationColor(CustomizationButtonType type,Color32 _value)
    {
        switch (type)
        {
            case CustomizationButtonType.Body:
                userInformation.SetBodyColor(_value);
                break;
            case CustomizationButtonType.Eyes:
                userInformation.SetEyesColor(_value);
                break;
            case CustomizationButtonType.Arms:
                userInformation.SetArmsColor(_value);
                break;
        }
        FirebaseManager.Instance.Save();
    }


    public void SetCoinCount(int value) => userInformation.SetCoin(value);
}