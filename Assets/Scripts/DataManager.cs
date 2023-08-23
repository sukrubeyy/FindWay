 using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

public class DataManager : Singleton<DataManager>
{
    public UserInformation userInformation;
    public bool isSave;
    public bool isDeleted;
    private void Start()
    {
      userInformation.SetUserID();
    }

    public void Update()
    {
        if (isSave)
        {
            isSave = false;
            FirebaseManager.Instance.Save();
        }
        else if (isDeleted)
        {
            isDeleted = false;
            FirebaseManager.Instance.DeleteData();
        }
    }

    public void ResetPlayerData()
    {
        userInformation = new UserInformation();
        userInformation.IncreaseLevel();
        FirebaseManager.Instance.Save();
    }
    public void SetCoinCount(int value) => userInformation.SetCoin(value);
}