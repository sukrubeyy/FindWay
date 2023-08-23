﻿using System;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private DatabaseReference reference;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                GetUserInformationFromFirebaseDatabase();
            }
        });
    }

    public void GetUserInformationFromFirebaseDatabase()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users").Child(DataManager.Instance.userInformation.GetUserId)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    string json = snapshot.GetRawJsonValue();
                    var cloudData =  JsonUtility.FromJson<UserInformation>(json);
                    if (cloudData is not null)
                    {
                        Debug.LogWarning("Firebase User Data Not Null");
                        DataManager.Instance.userInformation = cloudData;
                        MenuManager.Instance.IntializeElementsOfUI();
                    }
                    else
                    {
                        Debug.LogWarning("Firebase User Data Null");
                        DataManager.Instance.userInformation.Initialize();
                    }
                }
            });
    }

    public void DeleteData()
    {
        reference.Child("Users").Child(DataManager.Instance.userInformation.GetUserId).RemoveValueAsync();
        Debug.LogWarning("User Data Delete From Cloud");
        DataManager.Instance.userInformation.SetUserID();
        DataManager.Instance.userInformation.Initialize();
        MenuManager.Instance.IntializeElementsOfUI();
    }
    public void Save()
    {
        reference.Child("Users").Child(DataManager.Instance.userInformation.GetUserId).SetRawJsonValueAsync(JsonUtility.ToJson(DataManager.Instance.userInformation));
        GetUserInformationFromFirebaseDatabase();
        MenuManager.Instance.IntializeElementsOfUI();
        Debug.Log("SAVED");
    }
}