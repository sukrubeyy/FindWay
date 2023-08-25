using System;

public class DataManager : Singleton<DataManager>
{
    public UserInformation userInformation;

    private void Start()
    {
        userInformation.SetUserID();
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


    public void SetCoinCount(int value) => userInformation.SetCoin(value);
}